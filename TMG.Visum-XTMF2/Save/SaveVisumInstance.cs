namespace TMG.Visum.Save;

[Module(
    Description = "Save the current Visum instance to a file.",
    Name = "Save Visum Instance",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Save/SaveVisumInstance.html"
    )]
public sealed class SaveVisumInstance : BaseAction<VisumInstance>
{
    [Parameter(Name = "Save To", DefaultValue = "", Description = "The path to save the Visum instance to.", Index = 0)]
    public IFunction<string> SaveTo = null!;

    public override void Invoke(VisumInstance instance)
    {
        try
        {
            var savePath = Path.GetFullPath(SaveTo.Invoke());
            instance.SaveVersionFile(savePath);
        }
        catch (Exception ex)
        {
            throw new XTMFRuntimeException(this, "Unable to save Visum instance", ex);
        }
    }

    public override bool RuntimeValidation(ref string? error)
    {
        var savePath = SaveTo.Invoke();
        if (string.IsNullOrWhiteSpace(savePath))
        {
            error = "The save path must be non-empty!";
            return false;
        }
        return true;
    }
}
