namespace TMG.Visum.Load;

[Module(Name = "Load General Settings",
    Description = "This module loads the General Procedure Settings from a given file into the current instance.",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Load/LoadGeneralSettings.html")]
public sealed class LoadGeneralSettings : BaseAction<VisumInstance>
{
    [Parameter(Name = "File Path", DefaultValue = "", Description = "The location of the general settings file to load.", Index = 0)]
    public IFunction<string> FilePath = null!;


    [Parameter(Name = "Check Exists At Validation", Description = "Whether to check if the file exists during validation.", DefaultValue = "true", Index = 1)]
    public IFunction<bool> CheckExistsAtValidation = null!;

    public override void Invoke(VisumInstance context)
    {
        var path = FilePath.Invoke();
        try
        {
            context.LoadGeneralSettings(path);
        }
        catch (Exception ex)
        {
            throw new XTMFRuntimeException(this, $"Unable to load general settings from '{path}'\r\n{ex.Message}", ex);
        }
    }

    public override bool RuntimeValidation(ref string? error)
    {
        if (CheckExistsAtValidation?.Invoke() == true)
        {
            try
            {
                if (!File.Exists(FilePath.Invoke()))
                {
                    error = $"File '{FilePath.Invoke()}' does not exist.";
                    return false;
                }
            }
            catch(Exception ex)
            {
                error = $"Error checking file existence: {ex.Message}";
                return false;
            }
        }
        return true;
    }
}
