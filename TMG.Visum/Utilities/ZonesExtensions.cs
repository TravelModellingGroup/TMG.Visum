namespace TMG.Visum.Utilities;

/// <summary>
/// This class provides extensions for the Visum IZones interface
/// </summary>
internal static class ZonesExtensions
{
    public static int[] GetZoneNumbers(this IZones us)
    {
        List<int> zoneNumbers = new(us.Count);
        foreach (IZone zone in us)
        {
            zoneNumbers.Add(zone.ZoneNumber());
        }
        return zoneNumbers.ToArray();
    }

}
