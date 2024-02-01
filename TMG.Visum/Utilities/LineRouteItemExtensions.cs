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

}
