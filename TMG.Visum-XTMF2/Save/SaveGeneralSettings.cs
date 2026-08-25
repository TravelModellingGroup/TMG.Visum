namespace TMG.Visum.Save;

[Module(
    Name = "Save General Settings",
    Description = "This module saves the General Procedure Settings from the current instance to a given file.",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Save/SaveGeneralSettings.html"
    )]
public sealed class SaveGeneralSettings : BaseAction<VisumInstance>
{

    [Parameter(Name = "Save To", Description = "The file path to save the general settings to.", DefaultValue = "", Index = 0)]
    public IFunction<string> SaveTo = null!;

    public override void Invoke(VisumInstance context)
    {
        try
        {
            context.SaveGeneralSettings(SaveTo.Invoke());
        }
        catch (VisumException ex)
        {
            throw new XTMFRuntimeException(this, $"Failed to save general settings to '{SaveTo.Invoke()}'\r\n{ex.Message}.", ex);
        }
    }

}
