namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Execute an EditAttribute command
    /// </summary>
    /// <param name="formula">The formula to apply.</param>
    /// <param name="netObjectType">The network object type name to assign to.</param>
    /// <param name="resultAttributeName">The attribute name to assign to.</param>
    /// <param name="onlyActive">Should we only edit actively selected objects?</param>
    public void RunEditAttribute(string formula, string netObjectType, string resultAttributeName, bool onlyActive)
    {
        _lock.EnterWriteLock();
        string? tempFileName = null;
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            tempFileName = WriteProcedure((writer) =>
            {
                writer.WriteStartElement("OPERATION");
                writer.WriteAttributeString("ACTIVE", "1");
                writer.WriteAttributeString("NO", "1");
                writer.WriteAttributeString("OPERATIONTYPE", "EditAttribute");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                writer.WriteStartElement("ATTRIBUTEFORMULAPARA");

                writer.WriteAttributeString("FORMULA", formula);
                writer.WriteAttributeString("INCLUDESUBCATEGORIES", "0");
                writer.WriteAttributeString("NETOBJECTTYPE", netObjectType);
                writer.WriteAttributeString("ONLYACTIVE", onlyActive ? "1" : "0");
                writer.WriteAttributeString("RESULTATTRNAME", resultAttributeName);

                // end ATTRIBUTEFORMULAPARA
                writer.WriteEndElement();
            });
            // Wipe out the previous procedures and run this.
            _visum.Procedures.OpenXmlWithOptions(tempFileName);
            _visum.Procedures.Execute();
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
            _lock.ExitWriteLock();
        }
    }
}
