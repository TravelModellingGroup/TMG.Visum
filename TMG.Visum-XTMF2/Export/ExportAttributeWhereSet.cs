namespace TMG.Visum.Export;

[Module(
    Description = "This attribute will export the results of another attribute where the condition attribute is non-zero or non-empty. " +
        "The attribute will be saved in a CSV file with two columns, the first with the condition variable's value, the second with the exported attribute associated with it. " +
        "Both attributes need to be in the same domain.",
    Name = "Export Attribute Where Set",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Export/ExportAttributeWhereSet.html"
    )]
public sealed class ExportAttributeWhereSet : BaseAction<VisumInstance>
{
    [Parameter(Name = "Condition Attribute", DefaultValue = "", Description = "The name of the attribute to use as the condition.", Index = 0)]
    public IFunction<string> ConditionAttribute = null!;

    [Module(
        Description = "A simple module to provide the name of an attribute to use for an algorithm.",
        Name = "Attribute",
        DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Export/ExportAttributeWhereSet.html"
        )]
    public sealed class Attribute : IModule
    {
        [Parameter(Name = "Attribute Name", DefaultValue = "", Description = "The name of the attribute to export.", Index = 0)]
        public IFunction<string> AttributeName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            var attributeName = AttributeName.Invoke();
            if (string.IsNullOrWhiteSpace(attributeName))
            {
                error = "The attribute name must be set!";
                return false;
            }
            return true;
        }

        public string? Name { get; set; }
    }

    [SubModule(Name = "Export Attributes", Required = true, Description = "The name of the attributes to export.", Index = 1)]
    public Attribute[] ExportAttributes = null!;

    [Parameter(Name = "Network Object Type", DefaultValue = "Link", Description = "The type of network object of the condition and export attribute.", Index = 2)]
    public IFunction<NetworkObjectType> Type = null!;

    [Parameter(Name = "Save To", DefaultValue = "", Description = "The location to save the attribute to.", Index = 3)]
    public IFunction<string> SaveTo = null!;

    public override void Invoke(VisumInstance visumInstance)
    {
        try
        {
            var exportNames = ExportAttributes.Select(at => at.AttributeName.Invoke()).ToArray();
            var conditionAttribute = ConditionAttribute.Invoke();
            var type = Type.Invoke();
            var savePath = Path.GetFullPath(SaveTo.Invoke());
            visumInstance.ExportAttributeWhereSet(savePath, conditionAttribute, exportNames, type);
        }
        catch (Exception ex)
        {
            throw new XTMFRuntimeException(this, "Unable to export attribute", ex);
        }
    }

    public override bool RuntimeValidation(ref string? error)
    {
        var conditionAttribute = ConditionAttribute.Invoke();
        if (string.IsNullOrWhiteSpace(conditionAttribute))
        {
            error = "The condition attribute must be set!";
            return false;
        }
        var savePath = SaveTo.Invoke();
        if (string.IsNullOrWhiteSpace(savePath))
        {
            error = "The save path must be non-empty!";
            return false;
        }
        return true;
    }
}
