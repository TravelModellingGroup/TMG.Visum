namespace TMG.Visum.Utilities;

/// <summary>
/// This class provides extensions for the Visum IZones interface
/// </summary>
internal static class ZonesExtensions
{
    public static int[] GetZoneNumbers(object us)
    {
        dynamic zones = us;
        int[] ret = new int[zones.Count];
        int pos = 0;
        foreach (object zone in zones)
        {
            ret[pos++] = (int)(double)((dynamic)zone).AttValue["No"];
        }
        return ret;
    }

    public static (int[] zoneNumber, float[] x, float[] y) GetZoneInformation(object us)
    {
        dynamic zones = us;
        int[] zoneNumber = new int[zones.Count];
        float[] x = new float[zones.Count];
        float[] y = new float[zones.Count];
        int pos = 0;
        foreach (object zone in zones)
        {
            dynamic currentZone = zone;
            zoneNumber[pos] = (int)(double)currentZone.AttValue["No"];
            x[pos] = (float)(double)currentZone.AttValue["XCoord"];
            y[pos] = (float)(double)currentZone.AttValue["YCoord"];
            pos++;
        }
        return (zoneNumber, x, y);
    }

}
