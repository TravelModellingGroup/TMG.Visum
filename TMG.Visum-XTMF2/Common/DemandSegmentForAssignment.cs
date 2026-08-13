// Ignore Spelling: visum

namespace TMG.Visum;

[Module(
    Description = "Contains the information for setting up the demand segment that will be assigned.",
    Name = "Demand Segment For Assignment",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Common/DemandSegmentForAssignment.html"
    )]
public sealed class DemandSegmentForAssignment : BaseFunction<DemandSegmentForAssignment>
{
    [Parameter(Name = "Code",  DefaultValue = "C", Description = "The code for the demand segment.", Index = 0)]
    public IFunction<string> Code = null!;

    [Parameter(Name = "Demand Matrix",  DefaultValue = "", Description = "The name of the matrix that will be used for demand.", Index = 1)]
    public IFunction<string> DemandMatrix = null!;

    public override DemandSegmentForAssignment Invoke()
    {
        return this;
    }
}
