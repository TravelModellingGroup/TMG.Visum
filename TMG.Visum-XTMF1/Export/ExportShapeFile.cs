namespace TMG.Visum.Export;

[ModuleInformation(Description = "Export the currently loaded network to ShapeFile.")]
public sealed class ExportShapeFile : IVisumTool
{
    [RunParameter("Data Type", ShapeFileType.Link, "The type of data to export from the current VISUM network.")]
    public ShapeFileType Type;

    [SubModelInformation(Required = true, Description = "The root name of the ShapeFile to save to.")]
    public FileLocation SaveTo = null!;

    public void Execute(VisumInstance visumInstance)
    {
        try
        {
            visumInstance.ExportShapeFile(SaveTo, Type);
        }
        catch (Exception ex)
        {
            throw new XTMFRuntimeException(this, ex);
        }
    }

    public bool RuntimeValidation(ref string? error)
    {
        if(!Enum.IsDefined<ShapeFileType>(Type))
        {
            error = $"The type {Enum.GetName(Type)} is not a valid ShapeFile type.";
            return false;
        }
        return true;
    }

    public string Name { get; set; } = string.Empty;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new (50, 150, 50);
}
