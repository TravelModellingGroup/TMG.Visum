namespace TMG.Visum.Load;

[ModuleInformation(Description = "This module load the General Procedure Settings from a given file into the current instance.")]
public sealed class LoadGeneralSettings : IVisumTool
{
    [SubModelInformation(Required = true, Description = "The location of the general settings file to load.")]
    public FileLocation LoadFrom = null!;

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new (50,150,50);

    public void Execute(VisumInstance visumInstance)
    {
        try
        {
            visumInstance.LoadGeneralSettings(LoadFrom);
        }
        catch (Exception ex)
        {
            throw new XTMFRuntimeException(this, ex);
        }
    }

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

}
