namespace TMG.Visum.Export;

[Module(
    Description = "Export line boardings from the current VISUM instance to a CSV file.",
    Name = "Export Line Boardings",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Export/ExportLineBoardings.html"
    )]
public sealed class ExportLineBoardings : BaseAction<VisumInstance>
{
    [Parameter(Name = "Save To", DefaultValue = "", Description = "The location to save the line boardings to.", Index = 0)]
    public IFunction<string> SaveTo = null!;

    public override void Invoke(VisumInstance visumInstance)
    {
        List<(string lineName, float boardings)> boardings;
        try
        {
            boardings = visumInstance.GetBoardings();
        }
        catch (VisumException e)
        {
            throw new XTMFRuntimeException(this, "Unable to get boardings", e);
        }
        var savePath = Path.GetFullPath(SaveTo.Invoke());
        using var writer = new StreamWriter(savePath);
        writer.WriteLine("LineName,Boardings");
        foreach (var boarding in boardings)
        {
            writer.Write(boarding.lineName);
            writer.Write(',');
            writer.WriteLine(boarding.boardings);
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
