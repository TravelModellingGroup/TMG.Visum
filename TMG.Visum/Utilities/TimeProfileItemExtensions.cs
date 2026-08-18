namespace TMG.Visum.Utilities;

internal static class TimeProfileItemExtensions
{

    /// <summary>
    /// Get the accumulated run time for the time profile item.
    /// The result is in seconds.
    /// </summary>
    /// <param name="us">The time profile item to operate on.</param>
    /// <returns>The accumulated run time in seconds.</returns>
    public static double GetAccumulatedRunTime(object us)
    {
        return (double)((dynamic)us).AttValue["AccumRunTime"];
    }

    /// <summary>
    /// Get the accumulated run distance for the time profile item.
    /// </summary>
    /// <param name="us">The time profile item to operate on.</param>
    /// <returns>The accumulated run distance.</returns>
    public static double GetAccumulatedRunDistance(object us)
    {
        return (double)((dynamic)us).AttValue["AccumLength"];
    }

}
