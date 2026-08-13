using TMG.Visum.TransitAssignment;

namespace TMG.Visum.Assign;

/// <summary>
/// Provides an interface for selecting between different
/// transit assignment algorithms.
/// </summary>
public abstract class TransitAssignmentAlgorithmModule : BaseFunction<TransitAssignmentAlgorithmModule>
{
    /// <summary>
    /// Call this to get the transit assignment algorithm parameters that will be used.
    /// </summary>
    /// <returns></returns>
    internal abstract TransitAlgorithmParameters GetTransitParameters();

    public override TransitAssignmentAlgorithmModule Invoke()
    {
        return this;
    }

}
