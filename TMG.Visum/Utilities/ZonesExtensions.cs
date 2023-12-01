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

    public static (int[] zoneNumber, float[] x, float[] y) GetZoneInformation(this IZones us)
    {
        int[] zoneNumber = new int[us.Count];
        float[] x = new float[us.Count];
        float[] y = new float[us.Count];
        int pos = 0;
        foreach (IZone zone in us)
        {
            zoneNumber[pos] = zone.ZoneNumber();
            x[pos] = (float)zone.X();
            y[pos] = (float)zone.Y();
            pos++;
        }
        return (zoneNumber, x, y);
    }

}
