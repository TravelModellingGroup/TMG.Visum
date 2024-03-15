using TMG.Visum.TransitAssignment;

namespace TMG.Visum.Assign;

[ModuleInformation(Description = "Run a transit assignment.")]
public sealed class AssignTransitTool : IVisumTool
{
    [SubModelInformation(Required = true, Description = "The demand segments to execute in the road assignment.")]
    public DemandSegmentForAssignment[] DemandSegments = null!;

    [RunParameter("Auto Demand Segment", "C", "The demand segment that is used for STSU to base its times off of.")]
    public string AutoDemandSegment = null!;

    [ModuleInformation(Description = "The level of service matrices to generate.")]
    public sealed class LosMatrix : IModule
    {
        [RunParameter("LoS Type", PutLoSTypes.PerceivedJourneyTime, "The type of matrix to calculate.")]
        public PutLoSTypes Type;

        [RunParameter("Matrix Code", "", "The name to assign to the matrix.", Index = 0)]
        public string MatrixCode = null!;

        [RunParameter("Matrix Name", "", "The name to assign to the matrix.", Index = 1)]
        public string MatrixName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string Name { get; set; } = null!;

        public float Progress => 0f;

        public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

    }

    [SubModelInformation(Required = false, Description = "The different types of LoS to generate")]
    public LosMatrix[] LoSToGenerate = null!;

    [SubModelInformation(Required = true, Description = "The algorithm to use for the transit assignment.")]
    public TransitAssignmentAlgorithmModule AssignmentAlgorithm = null!;

    [RunParameter("Iterations", 1, "The number of times to execute the transit assignment, used for Surface-Transit Speed Updating.")]
    public int Iterations;

    public void Execute(VisumInstance instance)
    {
        List<VisumDemandSegment>? segments = null;
        List<List<VisumMatrix>>? processedMatrices = null;
        try
        {
            segments = GetDemandSegments(instance);
            var matricesToGenerate = LoSToGenerate.Select(matrix => matrix.Type).ToList();
            var transitParameters = AssignmentAlgorithm.GetTransitParameters();
            processedMatrices = instance.ExecuteTransitAssignment(segments, matricesToGenerate, transitParameters, Iterations);
            RenameMatrices(processedMatrices, instance);
        }
        catch (Exception e)
        {
            throw new XTMFRuntimeException(this, e);
        }
        finally
        {
            // Release the variables.
            if (segments is not null)
            {
                for (int i = 0; i < segments.Count; i++)
                {
                    segments[i]?.DemandMatrix?.Dispose();
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
        if (processedMatrices is null)
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
                    if (!string.IsNullOrWhiteSpace(LoSToGenerate[j].MatrixName))
                    {
                        RemoveDuplicatesAndSetName(processedMatrices[i][j], LoSToGenerate[j].MatrixName + " " + DemandSegments[i].Code);
                    }
                    if (!string.IsNullOrWhiteSpace(LoSToGenerate[j].MatrixCode))
                    {
                        processedMatrices[i][j].Code = LoSToGenerate[j].MatrixCode + " " + DemandSegments[i].Code;
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < processedMatrices[0].Count; j++)
            {
                if (!string.IsNullOrWhiteSpace(LoSToGenerate[j].MatrixName))
                {
                    RemoveDuplicatesAndSetName(processedMatrices[0][j], LoSToGenerate[j].MatrixName);
                }
                if (!string.IsNullOrWhiteSpace(LoSToGenerate[j].MatrixCode))
                {
                    processedMatrices[0][j].Code = LoSToGenerate[j].MatrixCode;
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
    private List<VisumDemandSegment> GetDemandSegments(VisumInstance instance)
    {
        return DemandSegments
            .Select(segment =>
            {
                var s = instance.GetDemandSegment(segment.Code);
                var matrix = instance.GetMatrixByName(segment.DemandMatrix);
                matrix.SetAsDemandMatrix();
                s.DemandMatrix = matrix;
                return s;
            })
            .ToList();
    }

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

}
