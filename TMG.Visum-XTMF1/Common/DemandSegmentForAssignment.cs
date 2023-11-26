// Ignore Spelling: visum

namespace TMG.Visum;

[ModuleInformation(
    Description = "Contains the information for setting up the demand segment that will be assigned."
    )]
public class DemandSegmentForAssignment : IModule
{
    [RunParameter("Code", "C", "The code for the demand segment.")]
    public string Code = null!;

    [RunParameter("Demand Matrix", "", "The name of the matrix that will be used for demand.")]
    public string DemandMatrix = null!;

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);
}

