// Ignore Spelling: visum

namespace TMG.Visum.Assign;

/// <summary>
/// Provides a generic abstraction for getting which road assignment algorithm to use.
/// </summary>
public abstract class RoadAssignmentAlgorithmModule : IModule
{
    internal protected abstract RoadAssignmentAlgorithm GetAlgorithm();

    public virtual bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);
}
