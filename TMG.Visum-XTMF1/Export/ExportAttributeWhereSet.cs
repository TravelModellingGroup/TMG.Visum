using XTMF;
namespace TMG.Visum.Export;

[ModuleInformation(Description = "This attribute will export the results of another attribute where the condition attribute is non-zero or non-empty." +
    " The attribute will be saved in a CSV file with two columns, the first with the condition variable's value, the second with the exported attribute associated with it." +
    " Both attributes need to be in the same domain.")]
public sealed class ExportAttributeWhereSet : IVisumTool
{
    [RunParameter("Condition Attribute", "", "The name of the attribute to use as the condition.")]
    public string ConditionAttribute = null!;

    [SubModelInformation(Required = true, Description = "The name of the attributes to export.")]
    public Attribute[] ExportAttributes = null!;

    [ModuleInformation(Description = "A simple module to provide the name of an attribute to use for an algorithm.")]
    public sealed class Attribute : IModule
    {

        [RunParameter("Attribute Name", "", "The name of the attribute to export.")]
        public string AttributeName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            if (string.IsNullOrWhiteSpace(AttributeName))
            {
                error = "The attribute name must be set!";
                return false;
            }
            return true;
        }

        public string Name { get; set; } = null!;

        public float Progress => 0f;

        public Tuple<byte, byte, byte> ProgressColour => new (50, 150, 50);
    }

    [RunParameter("Network Object Type", NetworkObjectType.Link, "The type of network object of the condition and export attribute.")]
    public NetworkObjectType Type = NetworkObjectType.Link;

    [SubModelInformation(Required = true, Description = "The location to save the attribute to.")]
    public FileLocation SaveTo = null!;

    public void Execute(VisumInstance visumInstance)
    {
        try
        {
            var exportNames = ExportAttributes.Select(at => at.AttributeName).ToArray();
            visumInstance.ExportAttributeWhereSet(SaveTo.GetFilePath(), ConditionAttribute, exportNames, Type);
        }
        catch (Exception ex)
        {
            throw new XTMFRuntimeException(this, ex);
        }
    }

    public bool RuntimeValidation(ref string? error)
    {
        if (string.IsNullOrWhiteSpace(ConditionAttribute))
        {
            error = "The condition attribute must be set!";
            return false;
        }
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

}
