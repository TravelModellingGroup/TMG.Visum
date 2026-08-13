using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Calculate;

[Module(
    Description = "Generates the level of service matrices for the given types. " +
        "Will fail if there is no road assignment previously.",
    Name = "Calculate Road LoS",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Calculate/CalculateRoadLoS.html"
    )]
public sealed class CalculateRoadLoS : BaseAction<VisumInstance>
{
    [SubModule(Name = "Segment", Required = true, Description = "The demand segment to calculate PrT for.", Index = 0)]
    public IFunction<DemandSegmentForAssignment> Segment = null!;

    [Parameter(Name = "Search Criterion", DefaultValue = "Impedance", Description = "The search criterion to use for the matrix.", Index = 1)]
    public IFunction<PrTLoSSearchCriterion> SearchCriterion = null!;

    [Module(Description = "Defines a PrT LoS matrix to export.", Name = "PrT LoS Export", DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Calculate/CalculateRoadLoS.html")]
    public sealed class PrTLoSExport : IModule
    {
        [Parameter(Name = "PrT Matrix Type", DefaultValue = "TCur", Description = "The type of matrix to compute from the previous road assignment.", Index = 0)]
        public IFunction<PrTLosTypes> Type = null!;

        [Parameter(Name = "Matrix Code", DefaultValue = "", Description = "If non-blank the matrix's code will be reassigned to the specified code.", Index = 1)]
        public IFunction<string> MatrixCode = null!;

        [Parameter(Name = "Matrix Name", DefaultValue = "", Description = "If non-blank the matrix will be renamed to the specified name.", Index = 2)]
        public IFunction<string> MatrixName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string? Name { get; set; }
    }

    [SubModule(Name = "To Export", Required = true, Description = "The types of matrices to export.", Index = 2)]
    public PrTLoSExport[] ToExport = null!;

    public override void Invoke(VisumInstance instance)
    {
        VisumDemandSegment? segment = null;
        try
        {
            segment = GetSegment(instance);
            var searchCriterion = SearchCriterion.Invoke();
            List<VisumMatrix> matrices = instance.CalculateRoadLoS(segment, ToExport.Select(type => type.Type.Invoke()).ToList(), searchCriterion);
            for (int i = 0; i < matrices.Count; i++)
            {
                var matrixCode = ToExport[i].MatrixCode.Invoke();
                if (!string.IsNullOrWhiteSpace(matrixCode))
                {
                    matrices[i].Code = matrixCode;
                }
                var newName = ToExport[i].MatrixName.Invoke();
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    // Make sure there is only one matrix with the given name.
                    if (!newName.Equals(matrices[i].Name, StringComparison.OrdinalIgnoreCase))
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
            throw new XTMFRuntimeException(this, "Unable to calculate road LoS", e);
        }
        finally
        {
            segment?.Dispose();
        }
    }

    private VisumDemandSegment GetSegment(VisumInstance instance)
    {
        var segment = Segment.Invoke();
        return instance.GetDemandSegment(segment.Code.Invoke());
    }
}
