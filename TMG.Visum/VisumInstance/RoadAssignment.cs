using TMG.Visum.RoadAssignment;

namespace TMG.Visum;

public partial class VisumInstance
{
    public void ExecuteRoadAssignment(VisumDemandSegment segment, RoadAssignmentAlgorithm algorithm)
    {
        ExecuteRoadAssignment([segment], algorithm);
    }

    public void ExecuteRoadAssignment(IEnumerable<VisumDemandSegment> demandSegments, RoadAssignmentAlgorithm algorithm)
    {
        CheckRoadAssignmentParameters(demandSegments, algorithm);

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
                writer.WriteAttributeString("OPERATIONTYPE", "PrTAssignment");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                writer.WriteStartElement("PRTASSIGNMENTPARA");
                writer.WriteAttributeString("DSEGSET", string.Join(',', demandSegments.Select(seg => seg.Code)));
                writer.WriteAttributeString("PRTASSIGNMENTVARIANT", algorithm.VariantName);
                algorithm.WriteParameters(writer);
                // end PRTASSIGNMENTPARA
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

    private static void CheckRoadAssignmentParameters(IEnumerable<VisumDemandSegment> demandSegments,
        RoadAssignmentAlgorithm algorithm)
    {
        // Before starting check if there are any parameters that don't make sense.
        algorithm.CheckParameters();

        // Throw exception if no demand segments exist
        if (!demandSegments.Any())
        {
            throw new VisumException("There were no demand segments defined!");
        }
    }

}
