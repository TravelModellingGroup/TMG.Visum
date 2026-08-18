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
    /// <returns>The attributes collection for the given network object type.</returns>
    internal static dynamic GetAttributes(this NetworkObjectType type, object instance)
    {
        dynamic visum = instance;
        return type switch
        {
            NetworkObjectType.Node => visum.Net.Nodes.Attributes,
            NetworkObjectType.Link => visum.Net.Links.Attributes,
            NetworkObjectType.TimeProfile => visum.Net.TimeProfiles.Attributes,
            _ => throw new NotImplementedException("Unknown NetworkObjectType"),
        };
    }

    internal static VISUMLIB.ValueType GetValueType(this AttributeTypes attributeType)
    {
        return attributeType switch
        {
            AttributeTypes.Float => VISUMLIB.ValueType.ValueType_Real,
            AttributeTypes.String => VISUMLIB.ValueType.ValueType_String,
            AttributeTypes.Integer => VISUMLIB.ValueType.ValueType_Int,
            _ => throw new NotImplementedException("Unknown AttributeTypes"),
        };
    }

    /// <summary>
    /// WRITE LOCK REQUIRED
    /// </summary>
    /// <param name="name">The name of the attribute to create.</param>
    /// <param name="netObjectType">The type of attribute to create.</param>
    internal static void CreateAttributeInternal(this NetworkObjectType type, object instance, string name, AttributeTypes attributeType)
    {
        dynamic visum = instance;
        var convertedType = GetValueType(attributeType);
        switch (type)
        {
            case NetworkObjectType.Node:
                visum.Net.Nodes.AddUserDefinedAttribute(name, name, name, convertedType);
                break;
            case NetworkObjectType.Link:
                visum.Net.Links.AddUserDefinedAttribute(name, name, name, convertedType);
                break;
            case NetworkObjectType.TimeProfile:
                visum.Net.TimeProfiles.AddUserDefinedAttribute(name, name, name, convertedType);
                break;
            default:
                throw new NotImplementedException("Unknown NetworkObjectType");
        }
    }

}
