namespace TMG.Visum.Utilities;

internal static class StopAreasExtensions
{
    public static int[] GetStopAreaNumbers(object us)
    {
        dynamic stopAreas = us;
        List<int> zoneNumbers = new(stopAreas.Count);
        foreach (object zone in stopAreas)
        {
            zoneNumbers.Add(StopAreaExtensions.StopNumber(zone));
        }
        return zoneNumbers.ToArray();
    }
}
