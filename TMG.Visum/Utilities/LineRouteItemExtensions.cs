namespace TMG.Visum.Utilities;

internal static class LineRouteItemExtensions
{

    /// <summary>
    /// Get the number of passengers boarding at this line route item.
    /// </summary>
    /// <param name="us">The line route item to process.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetBoardings(object us)
    {
        return (double)((dynamic)us).AttValue["PassBoard(AP)"];
    }

    /// <summary>
    /// Gets the inbound link associated with this line route item.
    /// </summary>
    /// <param name="us">The line route item to operate on.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static dynamic? GetInLink(object us, object instance)
    {
        var node = GetNode(us, instance);
        if (node is null)
        {
            return null;
        }
        var outLinkNo = ((dynamic)us).AttValue["INLINK\\NO"];
        if (outLinkNo is null)
        {
            return null;
        }
        return ((dynamic)instance).Net.Links.ItemByKey[node, outLinkNo];
    }

    /// <summary>
    /// Get the outbound link for this line route item. Might be null.
    /// </summary>
    /// <param name="us">The line route item to operate on.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static dynamic? GetOutLink(object us, object instance)
    {
        var nodeNumber = ((dynamic)us).AttValue["NODENO"];
        var outLinkToNode = ((dynamic)us).AttValue["OUTLINK\\TONODENO"];
        if (outLinkToNode is null)
        {
            return null;
        }
        return ((dynamic)instance).Net.Links.ItemByKey[nodeNumber, outLinkToNode];
    }

    /// <summary>
    /// Gets the node associated with this
    /// </summary>
    /// <param name="us">The line route item to operate on.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static dynamic? GetNode(object us, object instance)
    {
        var nodeNumber = ((dynamic)us).AttValue["NODENO"];
        return ((dynamic)instance).Net.Nodes.ItemByKey[nodeNumber];
    }

}
