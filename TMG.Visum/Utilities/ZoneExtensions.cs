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

}
