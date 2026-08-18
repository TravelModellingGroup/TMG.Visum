using TMG.Visum.Assign.RoadAlgorithms;
using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Assign;

[Module(
    Description = "Run a road assignment.",
    Name = "Assign Road",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Assign/AssignRoad.html"
    )]
public sealed partial class AssignRoad : BaseAction<VisumInstance>
{
    [SubModule(Name = "Demand Segments", Required = true, Description = "The demand segments to execute in the road assignment.", Index = 0)]
    public IFunction<DemandSegmentForAssignment>[] DemandSegments = null!;

    [SubModule(Name = "Road Assignment Algorithm", Required = false, Description = "Optionally specify the road assignment algorithm to use.", Index = 1)]
    public IFunction<RoadAssignmentAlgorithmModule>? RoadAssignmentAlgorithm;

    public override void Invoke(VisumInstance instance)
    {
        List<VisumDemandSegment>? segments = null;
        List<VisumMatrix>? demandMatrices = null;
        try
        {
            (segments, demandMatrices) = GetDemandSegments(instance);

            RoadAssignmentAlgorithm alg =
                (RoadAssignmentAlgorithm?.Invoke() ?? new LUCEAlgorithm()).GetAlgorithm(segments);

            instance.ExecuteRoadAssignment(segments, alg);
        }
        catch (VisumException e)
        {
            throw new XTMFRuntimeException(this, "Unable to execute road assignment", e);
        }
        finally
        {
            try
            {
                if (demandMatrices is not null)
                {
                    for (int i = 0; i < demandMatrices.Count; i++)
                    {
                        demandMatrices[i]?.Dispose();
                    }
                }
                if (segments is not null)
                {
                    for (int i = 0; i < segments.Count; i++)
                    {
                        segments[i].Dispose();
                    }
                }
            }
            catch
            { }
        }
    }

    /// <summary>
    /// Get the demand segments.
    /// 
    /// YOU MUST DISPOSE the segments after using them.
    /// </summary>
    /// <param name="instance">The visum instance to work for.</param>
    /// <returns>A list of VisumDemandSegments to use.</returns>
    private (List<VisumDemandSegment> Segments, List<VisumMatrix> DemandMatrices) GetDemandSegments(VisumInstance instance)
    {
        var segments = new List<VisumDemandSegment>(DemandSegments.Length);
        var matrices = new List<VisumMatrix>(DemandSegments.Length);
        foreach (var segmentFunc in DemandSegments)
        {
            var segment = segmentFunc.Invoke();
            var s = instance.GetDemandSegment(segment.Code.Invoke());
            var matrix = instance.GetMatrixByName(segment.DemandMatrix.Invoke());
            matrix.SetAsDemandMatrix();
            s.DemandMatrix = matrix;
            segments.Add(s);
            matrices.Add(matrix);
        }
        return (segments, matrices);
    }
}
