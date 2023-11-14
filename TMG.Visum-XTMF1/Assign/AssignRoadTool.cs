// Ignore Spelling: visum

using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Assign;

[ModuleInformation(Description = "Run a road assignment.")]
public sealed partial class AssignRoadTool : IVisumTool
{
    [RunParameter("Maximum Iterations", 100, "The maximum number of iterations to use when balancing the road assignment.", Index = 0)]
    public int MaximumIterations;

    [RunParameter("Max Gap", 0.01f, "The value is the weighted volume difference between the vehicle impedance of the network of the current iteration and the hypothetical vehicle impedance.", Index = 1)]
    public float MaxGap;

    [RunParameter("MaxRelative Link Impedance", 0.01f, "The maximum impedance on the link before we stop iterating.", Index = 2)]
    public float MaxRelativeLinkImpedance;

    [SubModelInformation(Required = true, Description = "The demand segments to execute in the road assignment.")]
    public DemandSegment[] DemandSegments = null!;

    [SubModelInformation(Required = false, Description = "Optionally specify the road assignment algorithm to use.")]
    public RoadAssignmentAlgorithmModule? RoadAssignmentAlgorithm;

    public void Execute(VisumInstance instance)
    {
        List<VisumDemandSegment>? segments = null;
        try
        {
            segments = GetDemandSegments(instance);
            instance.ExecuteRoadAssignment(segments, GetCriteria(), GetAssignmentAlgorithm());
        }
        catch (VisumException e)
        {
            throw new XTMFRuntimeException(this, e);
        }
        finally
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
                s.DemandMatrix = instance.GetMatrixByName(segment.DemandMatrix);
                return s;
            })
            .ToList();
    }

    private StabilityCriteria GetCriteria()
    {
        return new StabilityCriteria()
        {
            MaxIterations = MaximumIterations,
            MaxGap = MaxGap,
            MaxRelativeDifferenceLinkImpedance = MaxRelativeLinkImpedance,
        };
    }

    private RoadAssignmentAlgorithm GetAssignmentAlgorithm()
    {
        if (RoadAssignmentAlgorithm is not null)
        {
            return RoadAssignmentAlgorithm.GetAlgorithm();
        }
        return new LUCEAssignment() { };
    }

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

}
