namespace TMG.Visum.Utilities;

internal static class StopAreaExtensions
{
    /// <summary>
    /// Get the number of the stop area.
    /// </summary>
    /// <param name="stopArea"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int StopNumber(object stopArea)
    {
        return (int)(double)((dynamic)stopArea).AttValue["No"];
    }
}
