namespace TMG.Visum.Utilities;

internal static class ZoneExtensions
{
    /// <summary>
    /// Get the zone number.
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ZoneNumber(this IZone zone)
    {
        return (int)(double)zone.AttValue["No"];
    }

    /// <summary>
    /// Get the X coordinate for the zone.
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double X(this IZone zone)
    {
        return (double)zone.AttValue["XCoord"];
    }

    /// <summary>
    /// Get the Y coordinate for the zone.
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Y(this IZone zone)
    {
        return (double)zone.AttValue["YCoord"];
    }

}
