namespace TMG.Visum.Utilities;

internal static class StopAreasExtensions
{
    public static int[] GetStopAreaNumbers(this IStopAreas us)
    {
        List<int> zoneNumbers = new(us.Count);
        foreach (IStopArea zone in us)
        {
            zoneNumbers.Add(zone.StopNumber());
        }
        return zoneNumbers.ToArray();
    }
}
