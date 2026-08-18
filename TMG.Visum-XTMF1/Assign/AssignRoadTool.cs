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
        List<VisumMatrix>? demandMatrices = null;
        try
        {
            (segments, demandMatrices) = GetDemandSegments(instance);

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
        foreach (var segment in DemandSegments)
        {
            var s = instance.GetDemandSegment(segment.Code);
            var matrix = instance.GetMatrixByName(segment.DemandMatrix);
            matrix.SetAsDemandMatrix();
            s.DemandMatrix = matrix;
            segments.Add(s);
            matrices.Add(matrix);
        }
        return (segments, matrices);
    }


    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

}
