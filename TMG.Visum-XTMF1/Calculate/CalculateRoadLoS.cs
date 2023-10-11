using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Calculate;

[ModuleInformation(Description =
    "Generates the level of service matrices for the given types." +
    " Will fail if there is no road assignment previously."
)]
public sealed class CalculateRoadLoS : IVisumTool
{

    [SubModelInformation(Required = true, Description = "The demand segment to calculate PrT for.")]
    public DemandSegment Segment = null!;

    [RunParameter("PrT Matrix Type", PrTLosTypes.TCur, "The type of matrix to compute from the previous road assignment.")]
    public PrTLosTypes Type;

    public void Execute(VisumInstance instance)
    {
        VisumDemandSegment? segment = null;
        try
        {
            segment = GetSegment(instance);
            instance.CalculateRoadLoS(segment, Type);
        }
        catch (VisumException e)
        {
            throw new XTMFRuntimeException(this, e);
        }
        finally
        {
            segment?.Dispose();
        }
    }

    private VisumDemandSegment GetSegment(VisumInstance instance)
    {
        return instance.GetDemandSegment(Segment.Code);
    }

    public bool RuntimeValidation(ref string? error)
    {
        return true;   
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new (50,150,50);

}
