namespace TMG.Visum.Utilities;

internal static class LineRouteItemExtensions
{

    /// <summary>
    /// Get the number of passengers boarding at this line route item.
    /// </summary>
    /// <param name="us">The line route item to process.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetBoardings(this ILineRouteItem us)
    {
        return (double)us.AttValue["PassBoard(AP)"];
    }

    /// <summary>
    /// Gets the inbound link associated with this line route item.
    /// </summary>
    /// <param name="us">The line route item to operate on.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ILink? GetInLink(this ILineRouteItem us, IVisum instance)
    {
        var node = GetNode(us, instance);
        if (node is null)
        {
            return null;
        }
        var outLinkNo = us.AttValue["INLINK\\NO"];
        if (outLinkNo is null)
        {
            return null;
        }
        return instance.Net.Links.ItemByKey[node, outLinkNo];
    }

    /// <summary>
    /// Get the outbound link for this line route item. Might be null.
    /// </summary>
    /// <param name="us">The line route item to operate on.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ILink? GetOutLink(this ILineRouteItem us, IVisum instance)
    {
        var nodeNumber = us.AttValue["NODENO"];
        var outLinkToNode = us.AttValue["OUTLINK\\TONODENO"];
        if (outLinkToNode is null)
        {
            return null;
        }
        return instance.Net.Links.ItemByKey[nodeNumber, outLinkToNode];
    }

    /// <summary>
    /// Gets the node associated with this
    /// </summary>
    /// <param name="us">The line route item to operate on.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static INode? GetNode(this ILineRouteItem us, IVisum instance)
    {
        var nodeNumber = us.AttValue["NODENO"];
        var node = instance.Net.Nodes.ItemByKey[nodeNumber];
        return node as INode;
    }

}
