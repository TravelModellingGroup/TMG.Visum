using System.Diagnostics.CodeAnalysis;

namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Checks to see if an attribute exists with the given name.
    /// </summary>
    /// <param name="name">The name of the attribute to search for.</param>
    /// <param name="netObjectType">The type of network object the attribute belongs to.</param>
    /// <returns>True if the attribute exists, false otherwise.</returns>
    public bool CheckAttributeExists(string name, NetworkObjectType netObjectType)
    {
        _lock.EnterReadLock();
        try
        {
            return CheckAttributeExistsInternal(name, netObjectType);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Create a new user attribute if one does not already exist
    /// </summary>
    /// <param name="name">The name of the attribute to create if it doesn't already exist.</param>
    /// <param name="netObjectType">The type of network object that this will be associated with.</param>
    public void CreateAttributeIfDoesNotExist(string name, NetworkObjectType netObjectType)
    {
        _lock.EnterWriteLock();
        try
        {
            CreateAttributeIfDoesNotExistInternal(name, netObjectType);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// REQUIRES WRITE LOCK
    /// </summary>
    /// <param name="name">The name of the attribute to create.</param>
    internal void CreateAttributeIfDoesNotExistInternal(string name, NetworkObjectType netObjectType)
    {
        if (!TryGetAttributeInternal(name, netObjectType, out _))
        {
            netObjectType.CreateAttributeInternal(_visum!, name);
        }
    }

    /// <summary>
    /// Try to get an attribute by name for the given network object type.
    /// </summary>
    /// <param name="name">The name of the attribute to find.</param>
    /// <param name="netObjectType">The type of attribute to find.</param>
    /// <param name="attribute">The attribute if found, null otherwise.</param>
    /// <returns>True if the attribute was found, false otherwise.</returns>
    internal bool TryGetAttributeInternal(string name, NetworkObjectType netObjectType,
        [NotNullWhen(true)] out IAttribute? attribute)
    {
        try
        {
            var attributes = netObjectType.GetAttributes(_visum!);
            attribute = attributes.ItemByKey[name];
            return attribute is not null;
        }
        catch { }
        attribute = null;
        return false;
    }

    /// <summary>
    /// Check if an attribute with the given name exists.
    /// </summary>
    /// <param name="name">The name to check for.</param>
    /// <returns>True if the attribute already exists, false otherwise.</returns>
    internal bool CheckAttributeExistsInternal(string name, NetworkObjectType netObjectType)
    {
        return TryGetAttributeInternal(name, netObjectType, out var attribute)
            && attribute is not null;
    }

    /// <summary>
    /// Delete a user attribute from the given type of object given the name.
    /// </summary>
    /// <param name="name">The name of the user attribute to delete.</param>
    /// <param name="networkObjectType">The type of network object the attribute belongs to.</param>
    public void DeleteAttribute(string name, NetworkObjectType networkObjectType)
    {
        _lock.EnterWriteLock();
        try
        {
            DeleteAttributeInternal(name, networkObjectType);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Deletes an attribute if it exists
    /// REQUIRES WRITE LOCK.
    /// </summary>
    /// <param name="name">The name of the attribute to delete.</param>
    /// <param name="networkObjectType">The type of network object the attribute belongs to.</param>
    internal void DeleteAttributeInternal(string name, NetworkObjectType networkObjectType)
    {
        switch (networkObjectType)
        {
            case NetworkObjectType.Node:
                _visum!.Net.Nodes.DeleteUserDefinedAttribute(name);
                break;
            case NetworkObjectType.Link:
                _visum!.Net.Links.DeleteUserDefinedAttribute(name);
                break;
            case NetworkObjectType.TimeProfile:
                _visum!.Net.TimeProfiles.DeleteUserDefinedAttribute(name);
                break;
            default:
                throw new NotImplementedException("Unknown NetworkObjectType");
        }
    }

}
