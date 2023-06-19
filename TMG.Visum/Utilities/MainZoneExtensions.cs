namespace TMG.Visum.Utilities;

internal static class MainZoneExtensions
{
    /// <summary>
    /// Get the zone number for the main Zone
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ZoneNumber(this IMainZone zone)
    {
        return (int)zone.AttValue["No"];
    }
}
