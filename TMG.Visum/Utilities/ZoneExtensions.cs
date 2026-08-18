namespace TMG.Visum.Utilities;

internal static class ZoneExtensions
{
    /// <summary>
    /// Get the zone number.
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ZoneNumber(object zone)
    {
        return (int)(double)((dynamic)zone).AttValue["No"];
    }

    /// <summary>
    /// Get the X coordinate for the zone.
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double X(object zone)
    {
        return (double)((dynamic)zone).AttValue["XCoord"];
    }

    /// <summary>
    /// Get the Y coordinate for the zone.
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Y(object zone)
    {
        return (double)((dynamic)zone).AttValue["YCoord"];
    }

}
