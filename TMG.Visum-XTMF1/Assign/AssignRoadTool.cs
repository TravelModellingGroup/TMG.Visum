// Ignore Spelling: visum

using TMG.Visum.Assign.RoadAlgorithms;
using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Assign;

[ModuleInformation(Description = "Run a road assignment.")]
public sealed partial class AssignRoadTool : IVisumTool
{

    [SubModelInformation(Required = true, Description = "The demand segments to execute in the road assignment.")]
    public DemandSegmentForAssignment[] DemandSegments = null!;

    [SubModelInformation(Required = false, Description = "Optionally specify the road assignment algorithm to use.")]
    public RoadAssignmentAlgorithmModule? RoadAssignmentAlgorithm;

    public void Execute(VisumInstance instance)
    {
        List<VisumDemandSegment>? segments = null;
        try
        {
            segments = GetDemandSegments(instance);

            RoadAssignmentAlgorithm alg = 
                (RoadAssignmentAlgorithm ?? new LUCEAlgorithm()).GetAlgorithm(segments);

            instance.ExecuteRoadAssignment(segments, alg);
        }
        catch (VisumException e)
        {
            throw new XTMFRuntimeException(this, e);
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
            .Select(segment =>
            {
                var s = instance.GetDemandSegment(segment.Code);
                var matrix = instance.GetMatrixByName(segment.DemandMatrix);
                matrix.SetAsDemandMatrix();
                s.DemandMatrix = matrix;
                return s;
            })
            .ToList();
    }


    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

}
