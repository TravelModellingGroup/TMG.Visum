namespace TMG.Visum.Load;

[ModuleInformation(Description = "Loads the given matix number from the visum instance.")]
public class LoadMatrixFromVisum : IDataSource<SparseTwinIndex<float>>
{
    [RunParameter("Matrix Number", 0, "The number of the matrix to load.")]
    public int MatrixNumber;

    [SubModelInformation(Required = true, Description = "The instance of VISUM to load the matrix from.")]
    public IDataSource<VisumInstance> Visum = null!;

    private SparseTwinIndex<float>? _data;

    public void LoadData()
    {
        var instance = Visum.LoadInstance();
        VisumMatrix? matrix = null;
        try
        {
            if (!instance.TryGetMatrix(MatrixNumber, out matrix))
            {
                throw new XTMFRuntimeException(this, $"The matrix number {MatrixNumber} does not exist in the Visum instance!");
            }
            var matrixData = matrix.GetValuesAsFloatMatrix();
            var zones = matrix.GetSparseIndexes();
            _data = SparseTwinIndex<float>.CreateSquareTwinIndex(zones, matrixData);
        }
        catch (VisumException ex)
        {
            throw new XTMFRuntimeException(this, ex);
        }
        finally
        {
            matrix?.Dispose();
        }
    }

    public SparseTwinIndex<float>? GiveData()
    {
        return _data;
    }

    public void UnloadData()
    {
        _data = null;
    }

    public bool Loaded => _data is not null;

    public string Name { get; set; } = string.Empty;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }
}
