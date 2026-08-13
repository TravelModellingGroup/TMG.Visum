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
        try
        {
            segments = GetDemandSegments(instance);

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
                // Release the variables.
                if (segments is not null)
                {
                    for (int i = 0; i < segments.Count; i++)
                    {
                        segments[i]?.DemandMatrix?.Dispose();
                        segments[i].Dispose();
                    }
                }
            }
            catch // Kill all errors within the finally
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
    private List<VisumDemandSegment> GetDemandSegments(VisumInstance instance)
    {
        return DemandSegments
            .Select(segmentFunc =>
            {
                var segment = segmentFunc.Invoke();
                var s = instance.GetDemandSegment(segment.Code.Invoke());
                var matrix = instance.GetMatrixByName(segment.DemandMatrix.Invoke());
                matrix.SetAsDemandMatrix();
                s.DemandMatrix = matrix;
                return s;
            })
            .ToList();
    }
}
