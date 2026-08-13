using TMG.Visum.RoadAssignment;
using ChoiceModel = TMG.Visum.RoadAssignment.BicycleAssignment.BicycleChoiceModel;

namespace TMG.Visum.Assign.RoadAlgorithms;

[Module(
    Description = "Use the Visum Bicycle assignment with reasonable default values.",
    Name = "Bicycle Algorithm",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Assign/RoadAlgorithms/BicycleAlgorithm.html"
    )]
public sealed class BicycleAlgorithm : RoadAssignmentAlgorithmModule
{
    [Parameter(Name = "Search Iterations", DefaultValue = "10", Description = "Number of extra search iterations.", Index = 0)]
    public IFunction<int> NumSearchIterations = null!;

    [Parameter(Name = "Choice Model", DefaultValue = "Logit", Description = "Type of choice model to use. Reasonable default settings will be used for each.", Index = 1)]
    public IFunction<ChoiceModel> ChoiceModel = null!;

    [Parameter(Name = "Beta", DefaultValue = "0.25", Description = "For Logit or BoxCox model, parameter that multiplies the impedance before exponentiation.", Index = 2)]
    public IFunction<double> Beta = null!;

    public override RoadAssignmentAlgorithmModule Invoke()
    {
        return this;
    }

    protected internal override RoadAssignmentAlgorithm GetAlgorithm(List<VisumDemandSegment> segments)
    {
        return new BicycleAssignment(segments)
        {
            NumSearchIterations = NumSearchIterations.Invoke(),
            ChoiceModel = ChoiceModel.Invoke(),
            Beta = Beta.Invoke()
        };
    }
}
