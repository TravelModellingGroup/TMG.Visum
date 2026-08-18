namespace TMG.Visum.Load;

[Module(
    Name = "Load Visum Instance",
    Description = "This module is used for loading a new instance of VISUM",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Import/LoadVisumInstance.html"
    )]
public sealed class LoadVisumInstance : BaseFunction<VisumInstance>, IDisposable
{
    [SubModule(Required = false, Name = "Version File", Description = "An optional version file that we can load when creating the instance.", Index = 0)]
    public IFunction<string>? VersionFile;

    [Parameter(Name = "VISUM Version", DefaultValue = "2024", Description = "The VISUM major version to activate (supported: 2024, 2026).", Index = 1)]
    public IFunction<VisumVersion> VisumVersion = null!;

    private VisumInstance? _visumInstance;

    override public VisumInstance Invoke()
    {
        try
        {
            // TODO: Update Cache<T> to support disposing of the cached value when it is replaced instead of having it here.
            _visumInstance?.Dispose();
            var visumVersion = VisumVersion.Invoke();
            _visumInstance = VersionFile is not null
                ? new VisumInstance(Path.GetFullPath(VersionFile.Invoke()), visumVersion)
                : new VisumInstance(visumVersion);
            return _visumInstance;
        }
        catch (VisumException ex)
        {
            throw new XTMFRuntimeException(this, "Unable to load VISUM instance", ex);
        }
    }

    public void Dispose()
    {
        _visumInstance?.Dispose();
        _visumInstance = null;
        GC.SuppressFinalize(this);
    }
}