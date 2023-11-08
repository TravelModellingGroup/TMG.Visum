using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Assign.RoadAlgorithms;

[ModuleInformation(Description = "Use the Equilibrium assignment LUCE.")]
public sealed class LUCEAlgorithm : RoadAssignmentAlgorithmModule
{
    protected internal override RoadAssignmentAlgorithm GetAlgorithm()
    {
        return new LUCEAssignment() { };
    }
}
