namespace TMG.Visum.Export;

[ModuleInformation(Description = "")]
public sealed class ExportLineBoardings : IVisumTool
{

    [SubModelInformation(Required = true, Description = "The location to save the line boardings to.")]
    public FileLocation SaveTo = null!;

    public void Execute(VisumInstance visumInstance)
    {
        List<(string lineName, float boardings)> boardings;
        try
        {
             boardings = visumInstance.GetBoardings();
        }
        catch(VisumException e)
        {
            throw new XTMFRuntimeException(this, e);
        }
        using var writer = new StreamWriter(SaveTo);
        writer.WriteLine("LineName,Boardings");
        foreach(var boarding in boardings)
        {
            writer.Write(boarding.lineName);
            writer.Write(',');
            writer.WriteLine(boarding.boardings);
        }
    }

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new (50, 150, 50);

}
