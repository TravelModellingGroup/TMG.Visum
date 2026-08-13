using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Assign;

/// <summary>
/// Provides a generic abstraction for getting which road assignment algorithm to use.
/// </summary>
public abstract class RoadAssignmentAlgorithmModule : BaseFunction<RoadAssignmentAlgorithmModule>
{
    internal protected abstract RoadAssignmentAlgorithm GetAlgorithm(List<VisumDemandSegment> demandSegments);

    public override RoadAssignmentAlgorithmModule Invoke()
    {
        return this;
    }

}
