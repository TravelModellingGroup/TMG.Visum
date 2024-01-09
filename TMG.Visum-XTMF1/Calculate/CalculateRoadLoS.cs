using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Calculate;

[ModuleInformation(Description =
    "Generates the level of service matrices for the given types." +
    " Will fail if there is no road assignment previously."
)]
public sealed class CalculateRoadLoS : IVisumTool
{

    [SubModelInformation(Required = true, Description = "The demand segment to calculate PrT for.")]
    public DemandSegmentForAssignment Segment = null!;

    public class PrTLoSExport : IModule
    {
        [RunParameter("PrT Matrix Type", PrTLosTypes.TCur, "The type of matrix to compute from the previous road assignment.")]
        public PrTLosTypes Type;

        [RunParameter("Matrix Code", "", "If non-blank the matrix's code will be reassigned to the specified code.")]
        public string MatrixCode = null!;

        [RunParameter("Matrix Name", "", "If non-blank the matrix will be renamed to the specified name.")]
        public string MatrixName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string Name { get; set; } = null!;

        public float Progress => 0f;

        public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

    }

    [SubModelInformation(Required = true, Description = "The types of matrices to export.")]
    public PrTLoSExport[] ToExport = null!;

    public void Execute(VisumInstance instance)
    {
        VisumDemandSegment? segment = null;
        try
        {
            segment = GetSegment(instance);
            List<VisumMatrix> matrices = instance.CalculateRoadLoS(segment, ToExport.Select(type => type.Type).ToList());
            for (int i = 0; i < matrices.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(ToExport[i].MatrixCode))
                {
                    matrices[i].Code = ToExport[i].MatrixCode;
                }
                var newName = ToExport[i].MatrixName;
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    // Make sure there is only one matrix with the given name.
                    if (!ToExport[i].Name.Equals(matrices[i].Name, StringComparison.OrdinalIgnoreCase))
                    {
                        _ = instance.DeleteMatrixByName(newName);
                        matrices[i].Name = newName;
                    }
                }
                matrices[i].Dispose();
            }
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

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

}
