namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Exports a file where if the condition name is set (non zero or non-empty) for the given element, the export attribute is also stored.
    /// The resulting file will be a CSV file with two columns, the first with the condition variable's value, the second with the exported attribute associated with it.
    /// </summary>
    /// <param name="filePath">The file path to store the value to.  This should be an absolute path.</param>
    /// <param name="conditionName">The attribute to use as a condition.</param>
    /// <param name="exportNames">The attribute to export.</param>
    /// <param name="type">The type of network attribute both the condition name and export name should be of.</param>
    public void ExportAttributeWhereSet(string filePath, string conditionName, string[] exportNames, NetworkObjectType type)
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);

            // Step 1 - find the domain of the condition attribute
            (conditionName, string conditionParameter) = RemoveParameters(conditionName);
            if (!TryGetAttributeInternal(conditionName, type, out var conditionAttribute))
            {
                throw new VisumException($"The condition attribute {conditionName} does not exist within the domain of {Enum.GetName(type)}!");
            }

            bool conditionTypeIsNumber = conditionAttribute.ValueType switch
            {
                VISUMLIB.ValueType.ValueType_Int => true,
                VISUMLIB.ValueType.ValueType_DecimalPointT => true,
                _ => false,
            };
            var conditionCode = (string)conditionAttribute.get_Code() + conditionParameter;

            // Step 2 - Ensure that each of the export names do exist.
            var exportCodes = new string [exportNames.Length];
            for(int i = 0; i < exportNames.Length; i++)
            {
                (string withoutParameter, string parameters) = RemoveParameters(exportNames[i]);
                if (!TryGetAttributeInternal(withoutParameter, type, out var exportAttribute))
                {
                    throw new VisumException($"The export attribute {exportNames[i]} does not exist within the domain of {Enum.GetName(type)}!");
                }
                exportCodes[i] = (string)exportAttribute.get_Code() + parameters;
            }

            // Step 3 - create a filter for the domain of the condition attribute and set it to active
            ResetAllFilters();
            switch (type)
            {
                case NetworkObjectType.Link:
                    {
                        var filter = _visum.Filters.LinkFilter();
                        if (conditionTypeIsNumber)
                        {
                            filter.AddCondition("OP_NONE", false, conditionName, "GREATERVAL", 0.0);
                        }
                        else
                        {
                            filter.AddCondition("OP_NONE", true, conditionName, "ISEMPTY", null);
                        }
                        filter.UseFilter = true;
                    }
                    break;
                default:
                    throw new VisumException($"The network object type {Enum.GetName(type)} is not supported!");
            }


            // Step 4 - Enumerate the values of the condition attribute and the export attribute and save them to file
            {
                using var writer = new StreamWriter(filePath);
                writer.Write("Condition");
                foreach(var exportName in exportNames)
                {
                    writer.Write(',');
                    writer.Write(exportName);
                }
                writer.WriteLine();
                switch (type)
                {
                    case NetworkObjectType.Link:
                        {
                            var activeLinks = (object[])_visum.Net.Links.GetAllActive;
                            foreach (var l in activeLinks)
                            {
                                var link = (ILink)l;
                                var condition = link.AttValue[conditionCode];
                                writer.Write(condition);
                                foreach (var name in exportCodes)
                                {
                                    var export = link.AttValue[name];
                                    writer.Write(',');
                                    writer.Write(export);
                                }
                                writer.WriteLine();
                            }
                        }
                        break;
                    default:
                        throw new VisumException($"The network object type {Enum.GetName(type)} is not supported!");
                }
            }
            // Step 6 - Reset the active elements of the domain to their original state
            ResetAllFilters();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    private (string withoutCode, string code) RemoveParameters(string attributeName)
    {
        var startIndex = attributeName.IndexOf('(');
        if (startIndex == -1)
        {
            return (attributeName, string.Empty);
        }
        return (attributeName[..startIndex].Trim(), attributeName.Substring(startIndex, attributeName.Length - startIndex).Trim());
    }
}
