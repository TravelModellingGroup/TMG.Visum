namespace TMG.Visum.Load;

[Module(
    Description = "Tell the Visum instance to load the given version file.",
    Name = "Load Version File",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Load/LoadVersionFile.html"
    )]
public sealed class LoadVersionFile : BaseAction<VisumInstance>
{
    [Parameter(Name = "File To Load", DefaultValue = "", Description = "The version file for the Visum instance to load.", Index = 0)]
    public IFunction<string> ToLoad = null!;

    public override void Invoke(VisumInstance instance)
    {
        try
        {
            var filePath = Path.GetFullPath(ToLoad.Invoke());
            instance.LoadVersionFile(filePath);
        }
        catch (VisumException ex)
        {
            throw new XTMFRuntimeException(this, "Unable to load version file", ex);
        }
    }

    public override bool RuntimeValidation(ref string? error)
    {
        var filePath = ToLoad.Invoke();
        if (string.IsNullOrWhiteSpace(filePath))
        {
            error = "The file path must be non-empty!";
            return false;
        }
        return true;
    }
}
