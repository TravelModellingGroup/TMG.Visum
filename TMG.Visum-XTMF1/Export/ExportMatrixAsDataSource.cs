namespace TMG.Visum.Export;

[ModuleInformation(Description = "Load the given matrix by name from the VISUM instance.")]
public sealed class ExportMatrixAsDataSource : IDataSource<SparseTwinIndex<float>>
{
    [RootModule]
    public ITravelDemandModel Root = null!;

    [RunParameter("Matrix Name", "", "The name of the matrix to export.")]
    public string MatrixName = null!;

    [SubModelInformation(Required = true, Description = "The VISUM instance to export the matrix from.")]
    public IDataSource<VisumInstance> Instance = null!;

    public SparseTwinIndex<float>? _data;

    private int[] GetZoneSystemIndexes()
    {
        bool loaded = Root.ZoneSystem.Loaded;
        if (!loaded)
        {
            Root.ZoneSystem.LoadData();
        }
        var zones = Root.ZoneSystem.GiveData();
        if (!loaded)
        {
            Root.ZoneSystem.UnloadData();
        }
        return zones!.ZoneArray.ValidIndexArray();
    }

    public bool RuntimeValidation(ref string? error)
    {
        if (string.IsNullOrEmpty(MatrixName))
        {
            error = "The name of the matrix to export can not be blank!";
            return false;
        }
        if (Root.ZoneSystem is null)
        {
            error = "This module will require that the zone system has been filled out in order to learn what indexes are required.";
            return false;
        }
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

    public SparseTwinIndex<float>? GiveData()
    {
        return _data;
    }

    public void LoadData()
    {
        var loaded = Instance.Loaded;
        var instance = Instance.LoadInstance();
        if (!instance.TryGetMatrixByName(MatrixName, out var matrix) || matrix is null)
        {
            throw new XTMFRuntimeException(this, "There was no matrix with the name");
        }
        var data = matrix.GetValuesAsFloatMatrix();
        var sparseMatrix = SparseTwinIndex<float>.CreateSquareTwinIndex(GetZoneSystemIndexes(), data);
        _data = sparseMatrix;
        if (!loaded)
        {
            Instance.UnloadData();
        }
    }

    public void UnloadData()
    {
        _data = null;
    }

    public bool Loaded => _data is not null;

}
