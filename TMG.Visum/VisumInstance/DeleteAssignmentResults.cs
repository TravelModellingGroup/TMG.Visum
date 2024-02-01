namespace TMG.Visum;

public partial class VisumInstance
{

    /// <summary>
    /// Execute the Delete Assignment Results tool.
    /// </summary>
    /// <param name="resetPrT">Should we delete the private transit data.</param>
    /// <param name="resetPuT">Should we delete the public transit data.</param>
    /// <exception cref="VisumException"></exception>
    public void ExecuteDeleteAssignmentResults(bool resetPrT, bool resetPuT)
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
                writer.WriteAttributeString("OPERATIONTYPE", "InitAssignment");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                writer.WriteStartElement("ASSIGNMENTINITPARA");
                writer.WriteAttributeString("INITPRT", resetPrT ? "1" : "0");
                writer.WriteAttributeString("INITPUT", resetPuT ? "1" : "0");
                // end ASSIGNMENTINITPARA
                writer.WriteEndElement();
                // end OPERATION
                writer.WriteEndElement();
            });
            // Wipe out the previous procedures and run this.
            RunProceduresFromFileInternal(tempFileName);
            // Now double check that there were no errors.

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
