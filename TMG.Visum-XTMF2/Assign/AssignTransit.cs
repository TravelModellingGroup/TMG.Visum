using TMG.Visum.TransitAssignment;

namespace TMG.Visum.Assign;

[Module(
    Description = "Run a transit assignment.",
    Name = "Assign Transit",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Assign/AssignTransit.html"
    )]
public sealed class AssignTransit : BaseAction<VisumInstance>
{
    [SubModule(Name = "Demand Segments", Required = true, Description = "The demand segments to execute in the transit assignment.", Index = 0)]
    public IFunction<DemandSegmentForAssignment>[] DemandSegments = null!;

    [Module(Description = "The level of service matrices to generate.", Name = "LoS Matrix", DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Assign/AssignTransit.html")]
    public sealed class LosMatrix : IModule
    {
        [Parameter(Name = "LoS Type", DefaultValue = "PerceivedJourneyTime", Description = "The type of matrix to calculate.", Index = 0)]
        public IFunction<PutLoSTypes> Type = null!;

        [Parameter(Name = "Matrix Code", DefaultValue = "", Description = "The code to assign to the matrix.", Index = 1)]
        public IFunction<string> MatrixCode = null!;

        [Parameter(Name = "Matrix Name", DefaultValue = "", Description = "The name to assign to the matrix.", Index = 2)]
        public IFunction<string> MatrixName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string? Name { get; set; }
    }

    [SubModule(Name = "LoS To Generate", Required = false, Description = "The different types of LoS to generate.", Index = 1)]
    public LosMatrix[] LoSToGenerate = null!;

    [SubModule(Name = "Assignment Algorithm", Required = true, Description = "The algorithm to use for the transit assignment.", Index = 2)]
    public IFunction<TransitAssignmentAlgorithmModule> AssignmentAlgorithm = null!;

    [Parameter(Name = "Iterations", DefaultValue = "1", Description = "The number of times to execute the transit assignment, used for Surface-Transit Speed Updating.", Index = 3)]
    public IFunction<int> Iterations = null!;

    public override void Invoke(VisumInstance instance)
    {
        List<VisumDemandSegment>? segments = null;
        List<VisumMatrix>? demandMatrices = null;
        List<List<VisumMatrix>>? processedMatrices = null;
        try
        {
            (segments, demandMatrices) = GetDemandSegments(instance);
            var matricesToGenerate = LoSToGenerate?.Select(matrix => matrix.Type.Invoke()).ToList() ?? new List<PutLoSTypes>();
            var transitParameters = AssignmentAlgorithm.Invoke().GetTransitParameters();
            var iterations = Iterations.Invoke();
            processedMatrices = instance.ExecuteTransitAssignment(segments, matricesToGenerate, transitParameters, iterations);
            RenameMatrices(processedMatrices, instance);
        }
        catch (Exception e)
        {
            throw new XTMFRuntimeException(this, "Unable to execute transit assignment", e);
        }
        finally
        {
            if (demandMatrices is not null)
            {
                for (int i = 0; i < demandMatrices.Count; i++)
                {
                    demandMatrices[i]?.Dispose();
                }
            }
            if (segments is not null)
            {
                for (int i = 0; i < segments.Count; i++)
                {
                    segments[i].Dispose();
                }
            }
            if (processedMatrices is not null)
            {
                foreach (var matrixList in processedMatrices)
                {
                    foreach (var matrix in matrixList)
                    {
                        matrix.Dispose();
                    }
                }
            }
        }
    }

    /// <summary>
    /// The matrices to rename using the LoSToGenerate.
    /// </summary>
    /// <param name="processedMatrices">The matrices to rename.</param>
    private void RenameMatrices(List<List<VisumMatrix>>? processedMatrices, VisumInstance instance)
    {
        if (processedMatrices is null || LoSToGenerate == null)
        {
            return;
        }
        void RemoveDuplicatesAndSetName(VisumMatrix matrix, string name)
        {
            // only update if the name is actually changed.
            if (!name.Equals(matrix.Name, StringComparison.OrdinalIgnoreCase))
            {
                _ = instance.DeleteMatrixByName(name);
                matrix.Name = name;
            }
        }
        // If it was a multi-class assignment we
        // are going to have to deal with adding the demand segment name
        if (DemandSegments.Length > 1)
        {
            for (int i = 0; i < processedMatrices.Count; i++)
            {
                for (int j = 0; j < processedMatrices[i].Count; j++)
                {
                    var matrixName = LoSToGenerate[j].MatrixName.Invoke();
                    if (!string.IsNullOrWhiteSpace(matrixName))
                    {
                        var segmentCode = DemandSegments[i].Invoke().Code.Invoke();
                        RemoveDuplicatesAndSetName(processedMatrices[i][j], matrixName + " " + segmentCode);
                    }
                    var matrixCode = LoSToGenerate[j].MatrixCode.Invoke();
                    if (!string.IsNullOrWhiteSpace(matrixCode))
                    {
                        var segmentCode = DemandSegments[i].Invoke().Code.Invoke();
                        processedMatrices[i][j].Code = matrixCode + " " + segmentCode;
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < processedMatrices[0].Count; j++)
            {
                var matrixName = LoSToGenerate[j].MatrixName.Invoke();
                if (!string.IsNullOrWhiteSpace(matrixName))
                {
                    RemoveDuplicatesAndSetName(processedMatrices[0][j], matrixName);
                }
                var matrixCode = LoSToGenerate[j].MatrixCode.Invoke();
                if (!string.IsNullOrWhiteSpace(matrixCode))
                {
                    processedMatrices[0][j].Code = matrixCode;
                }
            }
        }
    }

    /// <summary>
    /// Get the demand segments.
    /// 
    /// YOU MUST DISPOSE the segments after using them.
    /// </summary>
    /// <param name="instance">The VISUM instance to work for.</param>
    /// <returns>A list of VisumDemandSegments to use.</returns>
    private (List<VisumDemandSegment> Segments, List<VisumMatrix> DemandMatrices) GetDemandSegments(VisumInstance instance)
    {
        var segments = new List<VisumDemandSegment>(DemandSegments.Length);
        var matrices = new List<VisumMatrix>(DemandSegments.Length);
        foreach (var segmentFunc in DemandSegments)
        {
            var segment = segmentFunc.Invoke();
            var s = instance.GetDemandSegment(segment.Code.Invoke());
            var matrix = instance.GetMatrixByName(segment.DemandMatrix.Invoke());
            matrix.SetAsDemandMatrix();
            s.DemandMatrix = matrix;
            segments.Add(s);
            matrices.Add(matrix);
        }
        return (segments, matrices);
    }
}
