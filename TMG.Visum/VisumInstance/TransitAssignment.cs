// Ignore Spelling: Visum

using TMG.Visum.TransitAssignment;

namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Execute a transit assignment using a single demand segment
    /// </summary>
    /// <param name="segment">The demand segment to execute with.</param>
    /// <param name="loSToGenerate"></param>
    /// <param name="parameters"></param>
    /// <returns>A list of matrices for the demand segment for each LoS to Generate.</returns>
    /// <exception cref="VisumException"></exception>
    public List<VisumMatrix> ExecuteTransitAssignment(VisumDemandSegment segment, IList<PutLoSTypes> loSToGenerate,
        TransitAlgorithmParameters parameters)
    {
        return ExecuteTransitAssignment(new VisumDemandSegment[] { segment }, loSToGenerate, parameters)[0];
    }

    /// <summary>
    /// Execute a transit assignment using multiple demand segments.
    /// </summary>
    /// <param name="segments">The demand segments to execute with.</param>
    /// <returns>A list for each demand segment of all of the requested LoS matrices.</returns>
    /// <exception cref="VisumException"></exception>
    public List<List<VisumMatrix>> ExecuteTransitAssignment(IList<VisumDemandSegment> segments, IList<PutLoSTypes> loSToGenerate,
        TransitAlgorithmParameters parameters) 
    {
        CheckTransitAssignmentParameters(segments);

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
                writer.WriteAttributeString("OPERATIONTYPE", "PuTAssignment");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                writer.WriteStartElement("PUTASSIGNMENTPARABASE");
                writer.WriteAttributeString("DSEGSET", string.Join(',', segments.Select(seg => seg.Code)));
                writer.WriteAttributeString("PUTASSIGNMENTVARIANT", parameters.VariantName);
                parameters.Write(writer, loSToGenerate);
                
                // end PUTASSIGNMENTPARABASE
                writer.WriteEndElement();
                // end OPERATION
                writer.WriteEndElement();
            });
            // Wipe out the previous procedures and run this.
            _visum.Procedures.OpenXmlWithOptions(tempFileName);
            _visum.Procedures.Execute();
            // Now double check that there were no errors.
            var ret = new List<List<VisumMatrix>>();
            foreach (var demandSegment in segments)
            {
                var segmentMatrices = new List<VisumMatrix>();
                foreach (var loSType in loSToGenerate)
                {
                    var matrixName = loSType.GetMatrixName(demandSegment);
                    var matrix = GetMatrixByNameInner(matrixName);
                    segmentMatrices.Add(matrix);
                }
                ret.Add(segmentMatrices);
            }
            return ret;
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

    private static void CheckTransitAssignmentParameters(IList<VisumDemandSegment> segments)
    {
        if (!segments.Any())
        {
            throw new VisumException("There were no demand segments defined!");
        }
        if(segments.FirstOrDefault(seg => seg.DemandMatrix is null) is VisumDemandSegment noMatrix)
        {
            throw new VisumException($"The demand segment {noMatrix.Name} was not initialized with a demand matrix for assignment!");
        }

    }

}
