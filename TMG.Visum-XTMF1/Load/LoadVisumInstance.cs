namespace TMG.Visum.Load;

[ModuleInformation(Description = "This module is used for loading a new instance of VISUM")]
public class LoadVisumInstance : IDataSource<VisumInstance>, IDisposable
{
    private VisumInstance? _visumInstance;

    [SubModelInformation(Required = false, Description = "An optional network that we can load when creating the instance.")]
    public FileLocation? VersionFile;

    [RunParameter("VISUM Version", VisumVersion.Visum2024, "The VISUM major version to activate (supported: 2024, 2025, 2026).")]
    public VisumVersion VisumVersion = VisumVersion.Visum2024;

    public VisumInstance? GiveData()
    {
        return _visumInstance;
    }

    public void LoadData()
    {
        try
        {
            if (_visumInstance is not null)
            {
                _visumInstance.Dispose();
            }
            _visumInstance = VersionFile is not null
                ? new VisumInstance(VersionFile, VisumVersion)
                : new VisumInstance(VisumVersion);
        }
        catch (VisumException ex)
        {
            throw new XTMFRuntimeException(this, ex);
        }
    }

    public void UnloadData()
    {
        _visumInstance?.Dispose();
        _visumInstance = null;
    }

    public bool Loaded => _visumInstance is not null;

    public string Name { get; set; } = string.Empty;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            _visumInstance?.Dispose();
            _visumInstance = null;
            disposedValue = true;
        }
    }

    ~LoadVisumInstance()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
