using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Assign.RoadAlgorithms
{
    public abstract class EquilibriumAlgorithmRoot : RoadAssignmentAlgorithmModule
    {
        [RunParameter("Maximum Iterations", 100, "The maximum number of iterations to use when balancing the road assignment.", Index = 0)]
        public int MaximumIterations;

        [RunParameter("Max Gap", 0.01f, "The value is the weighted volume difference between the vehicle impedance of the network of the current iteration and the hypothetical vehicle impedance.", Index = 1)]
        public float MaxGap;

        [RunParameter("MaxRelative Link Impedance", 0.01f, "The maximum impedance on the link before we stop iterating.", Index = 2)]
        public float MaxRelativeLinkImpedance;

        protected StabilityCriteria stabilityCriteria;

        protected EquilibriumAlgorithmRoot()
        {
            stabilityCriteria = new()
            {
                MaxIterations = MaximumIterations,
                MaxGap = MaxGap,
                MaxRelativeDifferenceLinkImpedance = MaxRelativeLinkImpedance
            };
        }
    }

    [ModuleInformation(Description = "Use the standard Equilibrium assignment.")]
    public sealed class EquilibriumAlgorithm : EquilibriumAlgorithmRoot
    {
        protected internal override RoadAssignmentAlgorithm GetAlgorithm(List<VisumDemandSegment> _) => 
            new EquilibriumAssignment(stabilityCriteria) { };
    }

    [ModuleInformation(Description = "Use the Equilibrium assignment Bi-conjugate Frank-Wolfe.")]
    public sealed class BiConjugateFrankWolfeAlgorithm : EquilibriumAlgorithmRoot
    {
        protected internal override RoadAssignmentAlgorithm GetAlgorithm(List<VisumDemandSegment> _) =>
            new BWFAssignment(stabilityCriteria) { };  
    }

    [ModuleInformation(Description = "Use the Equilibrium assignment LUCE.")]
    public sealed class LUCEAlgorithm : EquilibriumAlgorithmRoot
    {
        protected internal override RoadAssignmentAlgorithm GetAlgorithm(List<VisumDemandSegment> _) => 
            new LUCEAssignment(stabilityCriteria) { };
    }
}