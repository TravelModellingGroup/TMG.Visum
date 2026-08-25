
namespace TMG.Visum.Save;

[ModuleInformation(
    Description = "This module saves the General Procedure Settings from the current instance to a given file."
    )]
public sealed class SaveGeneralSettings : IVisumTool
{
    [SubModelInformation(Required = true, Description = "The file path to save the general settings to.", Index = 0)]
    public FileLocation SaveTo = null!;

    public void Execute(VisumInstance visumInstance)
    {
        try
        {
            visumInstance.SaveGeneralSettings(SaveTo);
        }
        catch (VisumException ex)
        {
            throw new XTMFRuntimeException(this, ex, $"Unable to save general settings to '{SaveTo}'");
        }
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }
}
