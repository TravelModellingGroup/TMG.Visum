namespace TMG.Visum.Save;

[ModuleInformation(Description = "Save the current Visum instance to a file.")]
public sealed class SaveVisumInstanceTool : IVisumTool
{
    [SubModelInformation(Required = true, Description = "The path to save the Visum instance to.")]
    public FileLocation SaveTo = null!;

    public void Execute(VisumInstance instance)
    {
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
