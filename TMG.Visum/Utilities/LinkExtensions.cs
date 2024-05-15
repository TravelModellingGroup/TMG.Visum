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
    public static INode? GetFromNode(this ILink us)
    {
        return us.AttValue["FromNode"] as INode;
    }

    /// <summary>
    /// Get the leg from the from node if it exists.
    /// </summary>
    /// <param name="us">The link to operate on.</param>
    /// <returns></returns>
    public static ILeg? GetFromLeg(this ILink us)
    {
        return us.AttValue["FromNodeLeg"] as ILeg;
    }

    /// <summary>
    /// Get the node that this link goes to.
    /// </summary>
    /// <param name="us">The link to operate on.</param>
    /// <returns></returns>
    public static INode? GetToNode(this ILink us)
    {
        return us.AttValue["ToNode"] as INode;
    }

    /// <summary>
    /// Get the leg from the to node if it exists.
    /// </summary>
    /// <param name="us"></param>
    /// <returns></returns>
    public static ILeg? ToNodeLeg(this ILink us)
    {
        return us.AttValue["ToNodeLeg"] as ILeg;
    }

    /// <summary>
    /// Gets the reverse leg if one exists.
    /// </summary>
    /// <param name="us">The link to operate on.</param>
    /// <returns></returns>
    public static ILink? GetReverseLink(this ILink us)
    {
        return us.AttValue["ReverseLink"] as ILink;
    }

    /// <summary>
    /// Gets the length of the link.
    /// </summary>
    /// <param name="us">The link to operate on.</param>
    /// <returns></returns>
    public static double GetLength(this ILink us)
    {
        return (double)us.AttValue["Length"];
    }

}
