namespace TMG.Visum.Export;

[ModuleInformation(Description = "Export the currently loaded network to ShapeFile." +
    " The default set of attributes for the given type will be exported.  If you want to include additional attributes" +
    " you will need to include them as an 'Extra Attribute'.")]
public sealed class ExportShapeFile : IVisumTool
{
    [RunParameter("Data Type", ShapeFileType.Link, "The type of data to export from the current VISUM network.")]
    public ShapeFileType Type;

    [SubModelInformation(Required = true, Description = "The root name of the ShapeFile to save to.")]
    public FileLocation SaveTo = null!;

    [RunParameter("Exclusively Extra Attributes", true, "Should we only export the specified extra attributes? If false all attributes specified in the network object type's list view will be exported.")]
    public bool ExclusivelyExtraAttributes;

    [ModuleInformation(Description = "")]
    public sealed class ExtraAttribute : IModule
    {
        [RunParameter("Attribute Name", "", "The name of the attribute to use.")]
        public string AttributeName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            if(string.IsNullOrWhiteSpace(AttributeName))
            {
                error = "The attribute name cannot be blank or just whitespace.";
                return false;
            }
            return true;
        }

        public string Name { get; set; } = null!;

        public float Progress => 0f;

        public Tuple<byte, byte, byte> ProgressColour => new (50,150,50);
    }

    [SubModelInformation(Required = false, Description = "Optional extra attributes to include.")]
    public ExtraAttribute[] ExtraAttributes = null!;

    public void Execute(VisumInstance visumInstance)
    {
        try
        {
            visumInstance.ExportShapeFile(SaveTo, Type, ExtraAttributes.Select(e => e.AttributeName).ToArray(), ExclusivelyExtraAttributes);
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
