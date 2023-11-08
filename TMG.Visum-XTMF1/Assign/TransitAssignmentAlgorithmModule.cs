using TMG.Visum.TransitAssignment;

namespace TMG.Visum.Assign;

/// <summary>
/// Provides an interface for selecting between different
/// transit assignment algorithms.
/// </summary>
public abstract class TransitAssignmentAlgorithmModule : IModule
{
    public virtual bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    /// <summary>
    /// Call this to get the transit assignment algorithm parameters that will be used.
    /// </summary>
    /// <returns></returns>
    internal abstract TransitAlgorithmParameters GetTransitParameters();

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50,150,50);

}
