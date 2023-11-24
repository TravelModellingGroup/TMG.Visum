namespace TMG.Visum.Utilities;

/// <summary>
/// This class provides extensions for the Visum IZones interface
/// </summary>
internal static class ZonesExtensions
{
    public static int[] GetZoneNumbers(this IZones us)
    {
        int[] ret = new int[us.Count];
        int pos = 0;
        foreach (IZone zone in us)
        {
            ret[pos++] = zone.ZoneNumber();
        }
        return ret;
    }

}
