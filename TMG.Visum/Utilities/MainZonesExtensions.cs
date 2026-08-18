namespace TMG.Visum.Utilities;

internal static class MainZonesExtensions
{
    /// <summary>
    /// Get all of the zones the belong to the main zones.
    /// </summary>
    /// <param name="us"></param>
    /// <returns></returns>
    public static int[] GetZoneNumbers(object us)
    {
        dynamic zones = us;
        int[] zoneNumbers = new int[zones.Count];
        int pos = 0;
        foreach (object zone in zones)
        {
            zoneNumbers[pos++] = (int)(double)((dynamic)zone).AttValue["No"];
        }
        return zoneNumbers;
    }
}
