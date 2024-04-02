namespace TMG.Visum;

public partial class VisumInstance
{

    /// <summary>
    /// Execute an EditAttribute command
    /// </summary>
    /// <param name="parameters">The parameters required to run an edit attribute.</param>
    /// <param name="filterFile">An optional filter file to use during the edit parameter.</param>
    public void ExecuteEditAttribute(EditAttributeParameters parameters, string? filterFile = null)
    {
        _lock.EnterWriteLock();
        try
        {
            ExecuteEditAttributeInternal(parameters, filterFile);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Execute an EditAttribute command
    /// INTERNAL ONLY- Must have write lock before calling.
    /// </summary>
    /// <param name="parameters"></param>
    /// <exception cref="VisumException"></exception>
    internal void ExecuteEditAttributeInternal(EditAttributeParameters parameters, string? filterFile = null)
    {
        string? tempFileName = null;
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            if (filterFile is not null)
            {
                OpenFilterInner(filterFile);
            }
            tempFileName = WriteProcedure((writer) =>
            {
                writer.WriteStartElement("OPERATION");
                writer.WriteAttributeString("ACTIVE", "1");
                writer.WriteAttributeString("NO", "1");
                writer.WriteAttributeString("OPERATIONTYPE", "EditAttribute");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                writer.WriteStartElement("ATTRIBUTEFORMULAPARA");

                writer.WriteAttributeString("FORMULA", parameters.Formula);
                writer.WriteAttributeString("INCLUDESUBCATEGORIES", "0");
                writer.WriteAttributeString("NETOBJECTTYPE", parameters.NetObjectType);
                writer.WriteAttributeString("ONLYACTIVE", parameters.OnlyActive ? "1" : "0");
                writer.WriteAttributeString("RESULTATTRNAME", parameters.ResultAttributeName);

                // end ATTRIBUTEFORMULAPARA
                writer.WriteEndElement();
            });
            // Wipe out the previous procedures and run this.
            RunProceduresFromFileInternal(tempFileName);
            if (filterFile is not null)
            {
                ResetAllFilters();
            }
        }
        catch (VisumException)
        {
            // Just pass through VisumExceptions
            throw;
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            Files.SafeDelete(tempFileName);
        }
    }
}
