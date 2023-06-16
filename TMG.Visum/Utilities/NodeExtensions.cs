using System.Xml.Linq;


namespace TMG.Visum.Utilities;

/// <summary>
/// This class provides the 
/// </summary>
internal static class NodeExtensions
{

    /// <summary>
    /// Get the number for the node
    /// </summary>
    /// <param name="us">The node to look at.</param>
    /// <returns>The node number.</returns>
    public static double NodeNumber(this INode us) => us.AttValue["No"];

    /// <summary>
    /// Get the X coordinate for the node
    /// </summary>
    /// <param name="us">The node to look at.</param>
    /// <returns>The node's X coordinate.</returns>
    public static double X(this INode us) => (double)us.AttValue["XCOORD"];

    /// <summary>
    /// Get the Y coordinate for the node
    /// </summary>
    /// <param name="us">The node to look at.</param>
    /// <returns>The node's Y coordinate.</returns>
    public static double Y(this INode us) => (double)us.AttValue["YCOORD"];

    /// <summary>
    /// Get the Z coordinate for the node
    /// </summary>
    /// <param name="us">The node to look at.</param>
    /// <returns>The node's Z coordinate.</returns>
    public static double Z(this INode us) => (double)us.AttValue["ZCOORD"];

    /// <summary>
    /// Get the list of attributes that belong to the nodes.
    /// </summary>
    /// <param name="us">The nodes to look at.</param>
    /// <returns>The list of attributes that belong to the nodes.</returns>
    public static List<IAttribute> GetAttributes(this INodes us)
    {
        IAttributes attributes = us.Attributes;
        return attributes.GetAll.ToList();
    }
}
