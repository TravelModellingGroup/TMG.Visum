
using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Assign.RoadAlgorithms;

[ModuleInformation(Description = "Use the standard Equilibrium assignment.")]
public sealed class EquilibriumAlgorithm : RoadAssignmentAlgorithmModule
{
    protected internal override RoadAssignmentAlgorithm GetAlgorithm()
    {
        return new EquilibriumAssignment() { };
    }
}
