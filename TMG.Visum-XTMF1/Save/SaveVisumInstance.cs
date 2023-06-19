namespace TMG.Visum.Save;

[ModuleInformation(Description = "Save the current Visum instance to a file.")]
public sealed class SaveVisumInstance : ISelfContainedModule
{
    [SubModelInformation(Required = true, Description = "The instance of Visum to operate on.")]
    public IDataSource<VisumInstance> Visum = null!;

    [SubModelInformation(Required = true, Description = "The path to save the Visum instance to.")]
    public FileLocation SaveTo = null!;

    public void Start()
    {
        var instance = Visum.LoadInstance();
        try
        {
            instance.SaveVersionFile(SaveTo);
        }
        catch(Exception ex)
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
