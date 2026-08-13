using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Assign.RoadAlgorithms;

public abstract class EquilibriumAlgorithmRoot : RoadAssignmentAlgorithmModule
{
    [Parameter(Name = "Maximum Iterations", DefaultValue = "100", Description = "The maximum number of iterations to use when balancing the road assignment.", Index = 0)]
    public IFunction<int> MaximumIterations = null!;

    [Parameter(Name = "Max Gap", DefaultValue = "0.01", Description = "The value is the weighted volume difference between the vehicle impedance of the network of the current iteration and the hypothetical vehicle impedance.", Index = 1)]
    public IFunction<float> MaxGap = null!;

    [Parameter(Name = "Max Relative Link Impedance", DefaultValue = "0.01", Description = "The maximum impedance on the link before we stop iterating.", Index = 2)]
    public IFunction<float> MaxRelativeLinkImpedance = null!;

    protected StabilityCriteria GetStabilityCriteria() =>
        new()
        {
            MaxIterations = MaximumIterations.Invoke(),
            MaxGap = MaxGap.Invoke(),
            MaxRelativeDifferenceLinkImpedance = MaxRelativeLinkImpedance.Invoke()
        };
}

[Module(Description = "Use the standard Equilibrium assignment.", Name = "Equilibrium Algorithm", DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Assign/RoadAlgorithms/EquilibriumAlgorithm.html")]
public sealed class EquilibriumAlgorithm : EquilibriumAlgorithmRoot
{
    protected internal override RoadAssignmentAlgorithm GetAlgorithm(List<VisumDemandSegment> _) =>
        new EquilibriumAssignment(GetStabilityCriteria()) { };
}

[Module(Description = "Use the Equilibrium assignment Bi-conjugate Frank-Wolfe.", Name = "Bi-Conjugate Frank-Wolfe Algorithm", DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Assign/RoadAlgorithms/BiConjugateFrankWolfeAlgorithm.html")]
public sealed class BiConjugateFrankWolfeAlgorithm : EquilibriumAlgorithmRoot
{
    protected internal override RoadAssignmentAlgorithm GetAlgorithm(List<VisumDemandSegment> _) =>
        new BWFAssignment(GetStabilityCriteria()) { };
}

[Module(Description = "Use the Equilibrium assignment LUCE.", Name = "LUCE Algorithm", DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Assign/RoadAlgorithms/LUCEAlgorithm.html")]
public sealed class LUCEAlgorithm : EquilibriumAlgorithmRoot
{
    protected internal override RoadAssignmentAlgorithm GetAlgorithm(List<VisumDemandSegment> _) =>
        new LUCEAssignment(GetStabilityCriteria()) { };
}
