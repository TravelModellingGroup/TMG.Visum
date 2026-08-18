namespace TMG.Visum.Utilities;

internal static class MainZoneExtensions
{
    /// <summary>
    /// Get the zone number for the main Zone
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ZoneNumber(object zone)
    {
        return (int)(double)((dynamic)zone).AttValue["No"];
    }
}
