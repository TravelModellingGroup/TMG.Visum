namespace TMG.Visum.Load;

[ModuleInformation(Description = "Tell the Visum instance to load the given version file.")]
public sealed class LoadVersionFileTool : IVisumTool
{
    [SubModelInformation(Required = true, Description = "The version file for the Visum instance to load.")]
    public FileLocation ToLoad = null!;

    public void Execute(VisumInstance instance)
    {
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
