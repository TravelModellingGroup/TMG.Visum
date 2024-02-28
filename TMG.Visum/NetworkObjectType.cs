namespace TMG.Visum;

/// <summary>
/// Refers to a network object within VISUM
/// </summary>
public enum NetworkObjectType
{
    Node,
    Link,
    TimeProfile,

}

/// <summary>
/// Used to deal with the different types of network objects
/// available for use in a network calculator.
/// </summary>
internal static class NetworkObjectTypeHelper
{

    /// <summary>
    /// Get the attribute list for the given object type.
    /// </summary>
    /// <param name="type">The type of network object to work with</param>
    /// <param name="instance">The VISUM instance to work with.</param>
    /// <returns>The IAttributes for the given network object type.</returns>
    internal static IAttributes GetAttributes(this NetworkObjectType type, IVisum instance)
    {
        return type switch
        {
            NetworkObjectType.Node => instance.Net.Nodes.Attributes,
            NetworkObjectType.Link => instance.Net.Links.Attributes,
            NetworkObjectType.TimeProfile => instance.Net.TimeProfiles.Attributes,
            _ => throw new NotImplementedException("Unknown NetworkObjectType"),
        };
    }

    /// <summary>
    /// WRITE LOCK REQUIRED
    /// </summary>
    /// <param name="name">The name of the attribute to create.</param>
    /// <param name="netObjectType">The type of attribute to create.</param>
    internal static void CreateAttributeInternal(this NetworkObjectType type, IVisum instance, string name)
    {
        switch (type)
        {
            case NetworkObjectType.Node:
                instance.Net.Nodes.AddUserDefinedAttribute(name, name, name, VISUMLIB.ValueType.ValueType_Real);
                break;
            case NetworkObjectType.Link:
                instance.Net.Links.AddUserDefinedAttribute(name, name, name, VISUMLIB.ValueType.ValueType_Real);
                break;
            case NetworkObjectType.TimeProfile:
                instance.Net.TimeProfiles.AddUserDefinedAttribute(name, name, name, VISUMLIB.ValueType.ValueType_Real);
                break;
            default:
                throw new NotImplementedException("Unknown NetworkObjectType");
        }
    }

}
