using System.Xml.Linq;


namespace TMG.Visum.Utilities;

/// <summary>
/// This class provides the 
/// </summary>
public static class NodeExtensions
{

    /// <summary>
    /// Get the number for the node
    /// </summary>
    /// <param name="us">The node to look at.</param>
    /// <returns>The node number.</returns>
    public static double NodeNumber(object us) => (double)((dynamic)us).AttValue["No"];

    /// <summary>
    /// Get the X coordinate for the node
    /// </summary>
    /// <param name="us">The node to look at.</param>
    /// <returns>The node's X coordinate.</returns>
    public static double X(object us) => (double)((dynamic)us).AttValue["XCOORD"];

    /// <summary>
    /// Get the Y coordinate for the node
    /// </summary>
    /// <param name="us">The node to look at.</param>
    /// <returns>The node's Y coordinate.</returns>
    public static double Y(object us) => (double)((dynamic)us).AttValue["YCOORD"];

    /// <summary>
    /// Get the Z coordinate for the node
    /// </summary>
    /// <param name="us">The node to look at.</param>
    /// <returns>The node's Z coordinate.</returns>
    public static double Z(object us) => (double)((dynamic)us).AttValue["ZCOORD"];

    /// <summary>
    /// Get the list of attributes that belong to the nodes.
    /// </summary>
    /// <param name="us">The nodes to look at.</param>
    /// <returns>The list of attributes that belong to the nodes.</returns>
    public static List<dynamic> GetAttributes(object us)
    {
        dynamic attributes = ((dynamic)us).Attributes;
        List<dynamic> ret = [];
        foreach (dynamic attribute in attributes.GetAll)
        {
            ret.Add(attribute);
        }
        return ret;
    }
}
