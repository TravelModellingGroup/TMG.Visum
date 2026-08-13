namespace TMG.Visum.Export;

[Module(
    Description = "Export the currently loaded network to ShapeFile. " +
        "The default set of attributes for the given type will be exported. If you want to include additional attributes " +
        "you will need to include them as an 'Extra Attribute'.",
    Name = "Export Shape File",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Export/ExportShapeFile.html"
    )]
public sealed class ExportShapeFile : BaseAction<VisumInstance>
{
    [Parameter(Name = "Data Type", DefaultValue = "Link", Description = "The type of data to export from the current VISUM network.", Index = 0)]
    public IFunction<ShapeFileType> Type = null!;

    [Parameter(Name = "Save To", DefaultValue = "", Description = "The root name of the ShapeFile to save to.", Index = 1)]
    public IFunction<string> SaveTo = null!;

    [Parameter(Name = "Exclusively Extra Attributes", DefaultValue = "true", Description = "Should we only export the specified extra attributes? If false all attributes specified in the network object type's list view will be exported.", Index = 2)]
    public IFunction<bool> ExclusivelyExtraAttributes = null!;

    [Module(Description = "Represents an extra attribute to export to the shape file.", Name = "Extra Attribute", DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Export/ExportShapeFile.html")]
    public sealed class ExtraAttribute : IModule
    {
        [Parameter(Name = "Attribute Name", DefaultValue = "", Description = "The name of the attribute to use.", Index = 0)]
        public IFunction<string> AttributeName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            var attributeName = AttributeName.Invoke();
            if (string.IsNullOrWhiteSpace(attributeName))
            {
                error = "The attribute name cannot be blank or just whitespace.";
                return false;
            }
            return true;
        }

        public string? Name { get; set; }
    }

    [SubModule(Name = "Extra Attributes", Required = false, Description = "Optional extra attributes to include.", Index = 3)]
    public ExtraAttribute[] ExtraAttributes = null!;

    public override void Invoke(VisumInstance visumInstance)
    {
        try
        {
            var type = Type.Invoke();
            var savePath = Path.GetFullPath(SaveTo.Invoke());
            var exclusivelyExtraAttributes = ExclusivelyExtraAttributes.Invoke();
            var extraAttributes = ExtraAttributes?.Select(e => e.AttributeName.Invoke()).ToArray() ?? Array.Empty<string>();
            visumInstance.ExportShapeFile(savePath, type, extraAttributes, exclusivelyExtraAttributes);
        }
        catch (Exception ex)
        {
            throw new XTMFRuntimeException(this, "Unable to export shape file", ex);
        }
    }

    public override bool RuntimeValidation(ref string? error)
    {
        var type = Type.Invoke();
        if (!Enum.IsDefined<ShapeFileType>(type))
        {
            error = $"The type {Enum.GetName(type)} is not a valid ShapeFile type.";
            return false;
        }
        var exclusivelyExtraAttributes = ExclusivelyExtraAttributes.Invoke();
        if (exclusivelyExtraAttributes && (ExtraAttributes == null || ExtraAttributes.Length == 0))
        {
            error = "You cannot export exclusively extra attributes without specifying at least one extra attribute.";
            return false;
        }
        var savePath = SaveTo.Invoke();
        if (string.IsNullOrWhiteSpace(savePath))
        {
            error = "The save path must be non-empty!";
            return false;
        }
        return true;
    }
}
