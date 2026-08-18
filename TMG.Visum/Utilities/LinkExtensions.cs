namespace TMG.Visum.Utilities;

/// <summary>
/// Provides extensions when working with
/// ILink.
/// </summary>
internal static class LinkExtensions
{

    /// <summary>
    /// Get the node that this link starts from.
    /// </summary>
    /// <param name="us">The link to operate on.</param>
    /// <returns></returns>
    public static dynamic? GetFromNode(object us)
    {
        return ((dynamic)us).AttValue["FromNode"];
    }

    /// <summary>
    /// Get the leg from the from node if it exists.
    /// </summary>
    /// <param name="us">The link to operate on.</param>
    /// <returns></returns>
    public static dynamic? GetFromLeg(object us)
    {
        return ((dynamic)us).AttValue["FromNodeLeg"];
    }

    /// <summary>
    /// Get the node that this link goes to.
    /// </summary>
    /// <param name="us">The link to operate on.</param>
    /// <returns></returns>
    public static dynamic? GetToNode(object us)
    {
        return ((dynamic)us).AttValue["ToNode"];
    }

    /// <summary>
    /// Get the leg from the to node if it exists.
    /// </summary>
    /// <param name="us"></param>
    /// <returns></returns>
    public static dynamic? ToNodeLeg(object us)
    {
        return ((dynamic)us).AttValue["ToNodeLeg"];
    }

    /// <summary>
    /// Gets the reverse leg if one exists.
    /// </summary>
    /// <param name="us">The link to operate on.</param>
    /// <returns></returns>
    public static dynamic? GetReverseLink(object us)
    {
        return ((dynamic)us).AttValue["ReverseLink"];
    }

    /// <summary>
    /// Gets the length of the link.
    /// </summary>
    /// <param name="us">The link to operate on.</param>
    /// <returns></returns>
    public static double GetLength(object us)
    {
        return (double)((dynamic)us).AttValue["Length"];
    }

}
