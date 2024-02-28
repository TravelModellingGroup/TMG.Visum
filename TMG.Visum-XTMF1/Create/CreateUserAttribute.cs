
namespace TMG.Visum.Create;

[ModuleInformation(Description = "Create the user attribute if it does not exist.")]
public sealed class CreateUserAttribute : IVisumTool
{

    [RunParameter("Attribute Name", "AttributeName", "The name of the attribute to create.")]
    public string AttributeName = string.Empty;

    [RunParameter("Network Object Type", NetworkObjectType.Node, "The of network object to create the attribute for.")]
    public NetworkObjectType NetworkObjectType;

    public void Execute(VisumInstance visumInstance)
    {
        try
        {
            visumInstance.CreateAttributeIfDoesNotExist(AttributeName, NetworkObjectType);
        }
        catch (VisumException e)
        {
            throw new XTMFRuntimeException(this, e);
        }
    }

    public bool RuntimeValidation(ref string? error)
    {
        if (string.IsNullOrWhiteSpace(AttributeName))
        {
            error = "You must specify the Attribute Name in order to create it!";
            return false;
        }
        return true;
    }

    public string Name { get; set; } = string.Empty;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

}
