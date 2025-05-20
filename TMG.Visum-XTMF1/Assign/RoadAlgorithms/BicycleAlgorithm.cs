using TMG.Visum.RoadAssignment;
using ChoiceModel = TMG.Visum.RoadAssignment.BicycleAssignment.BicycleChoiceModel;

namespace TMG.Visum.Assign.RoadAlgorithms;


[ModuleInformation(Description = "Use the Visum Bicycle assignment with reasonable default values.")]
public sealed class BicycleAlgorithm : RoadAssignmentAlgorithmModule
{

    [RunParameter("Search iterations", 10, "Number of extra search iterations")]
    public int NumSearchIterations;

    [RunParameter("Choice model", ChoiceModel.Logit, "Type of choice model to use. Reasonable default settings will be used for each.")]
    public ChoiceModel ChoiceModel;

    [RunParameter("Beta", 0.25, "For Logit or BoxCox model, parameter that multiplies the impedence before exponentiation.")]
    public double Beta;

    protected internal override RoadAssignmentAlgorithm GetAlgorithm(List<VisumDemandSegment> segments)
    {
        return new BicycleAssignment(segments) { 
            NumSearchIterations = NumSearchIterations,
            ChoiceModel = ChoiceModel,
            Beta = Beta
        };
    }


}
