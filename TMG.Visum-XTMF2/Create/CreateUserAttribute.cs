
namespace TMG.Visum.Create;

[Module(Description = "Create the user attribute if it does not exist.",
    Name = "Create User Attribute",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Create/CreateUserAttribute.html"
    )]
public sealed class CreateUserAttribute : BaseAction<VisumInstance>
{

    [Parameter(Name = "Attribute Name", DefaultValue = "AttributeName", Description = "The name of the attribute to create.", Index = 0)]
    public IFunction<string> AttributeName = null!;

    [Parameter(Name = "Network Object Type", DefaultValue = "Node", Description = "The of network object to create the attribute for.", Index = 1)]
    public IFunction<NetworkObjectType> NetworkObjectType = null!;

    [Parameter(Name = "Attribute Type", DefaultValue = "Float", Description = "The type of the attribute to create.", Index = 2)]
    public IFunction<AttributeTypes> AttributeType = null!;

    public override void Invoke(VisumInstance visumInstance)
    {
        try
        {
            var attributeName = AttributeName.Invoke();
            var objectType = NetworkObjectType.Invoke();
            var attributeType = AttributeType.Invoke();
            visumInstance.CreateAttributeIfDoesNotExist(attributeName, objectType, attributeType);
        }
        catch (VisumException e)
        {
            throw new XTMFRuntimeException(this, "Unable to create attribute", e);
        }
    }

    public override bool RuntimeValidation(ref string? error)
    {
        var attributeName = AttributeName.Invoke();
        if (string.IsNullOrWhiteSpace(attributeName))
        {
            error = "You must specify the Attribute Name in order to create it!";
            return false;
        }
        return true;
    }

}
