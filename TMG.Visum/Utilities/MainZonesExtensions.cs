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
        List<int> zoneNumbers = new(us.Count);
        foreach (IMainZone zone in us)
        {
            zoneNumbers.Add(zone.ZoneNumber());
        }
        return zoneNumbers.ToArray();
    }
}
