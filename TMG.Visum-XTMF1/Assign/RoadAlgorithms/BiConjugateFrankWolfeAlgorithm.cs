
using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Assign.RoadAlgorithms;

[ModuleInformation(Description = "Use the Equilibrium assignment Bi-conjugate Frank-Wolfe.")]
public sealed class BiConjugateFrankWolfeAlgorithm : RoadAssignmentAlgorithmModule
{
    protected internal override RoadAssignmentAlgorithm GetAlgorithm()
    {
        return new BWFAssignment() { };
    }
}
