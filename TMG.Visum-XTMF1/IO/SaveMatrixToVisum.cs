﻿namespace TMG.Visum.IO;

[ModuleInformation(Description = "Store the given matrix into a Visum Instance.")]
public sealed class SaveMatrixToVisum : ISelfContainedModule
{
    [RunParameter("Matrix Number", 0, "The matrix number to store into.")]
    public int MatrixNumber;

    [RunParameter("Matrix Name", "", "The name to assosiate with the matrix.")]
    public string MatrixName = string.Empty;

    [SubModelInformation(Required = true, Description = "The matrix to Store into Visum")]
    public IDataSource<SparseTwinIndex<float>> ToSave = null!;

    [SubModelInformation(Required = true, Description = "The Visum instance to store the matrix to.")]
    public IDataSource<VisumInstance> Visum = null!;

    public void Start()
    {
        var instance = GetInstance();
        try
        {
            if (!instance.TryGetMatrix(MatrixNumber, out var matrix))
            {
                matrix = instance.CreateDemandMatrix(MatrixNumber, MatrixName);
            }
            else
            {
                matrix.Name = MatrixName;
            }
            matrix.SetValues(GetMatrix());
        }
        catch(VisumException ex)
        {
            throw new XTMFRuntimeException(this, ex);
        }
    }

    private float[][] GetMatrix()
    {
        var loaded = ToSave.Loaded;
        if(!loaded)
        {
            ToSave.LoadData();
        }
        var ret = ToSave.GiveData();
        if(!loaded)
        {
            ToSave.UnloadData();
        }
        return ret!.GetFlatData();
    }

    private VisumInstance GetInstance()
    {
        if (!Visum.Loaded)
        {
            Visum.LoadData();
        }
        return Visum.GiveData()!;
    }

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = string.Empty;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);
}
