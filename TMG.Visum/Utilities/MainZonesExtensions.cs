namespace TMG.Visum.Utilities;

internal static class MainZonesExtensions
{
    /// <summary>
    /// Get all of the zones the belong to the main zones.
    /// </summary>
    /// <param name="us"></param>
    /// <returns></returns>
    public static int[] GetZoneNumbers(this IMainZones us)
    {
        int[] zoneNumbers = new int[us.Count];
        int pos = 0;
        foreach (IMainZone zone in us)
        {
            zoneNumbers[pos++] = zone.ZoneNumber();
        }
        return zoneNumbers;
    }
}
