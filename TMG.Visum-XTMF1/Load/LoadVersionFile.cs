namespace TMG.Visum.Load;

[ModuleInformation(Description = "Tell the Visum instance to load the given version file.")]
public sealed class LoadVersionFile : ISelfContainedModule
{
    [SubModelInformation(Required = true, Description = "The instance of VISUM to load the matrix from.")]
    public IDataSource<VisumInstance> Visum = null!;

    [SubModelInformation(Required = true, Description = "The version file for the Visum instance to load.")]
    public FileLocation ToLoad = null!;

    public void Start()
    {
        var instance = Visum.LoadInstance();
        try
        {
            instance.LoadVersionFile(ToLoad);
        }
        catch (VisumException ex)
        {
            throw new XTMFRuntimeException(this, ex);
        }
    }

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = string.Empty;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);
}
