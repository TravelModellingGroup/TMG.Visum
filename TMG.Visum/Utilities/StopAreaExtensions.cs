namespace TMG.Visum.Utilities;

internal static class StopAreaExtensions
{
    /// <summary>
    /// Get the number of the stop area.
    /// </summary>
    /// <param name="stopArea"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int StopNumber(this IStopArea stopArea)
    {
        return (int)(double)stopArea.AttValue["No"];
    }
}
