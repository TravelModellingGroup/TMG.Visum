namespace TMG.Visum.Utilities;

internal static class StandardTimeSeriesExtensions
{
    /// <summary>
    /// Get the weight of the time series item
    /// </summary>
    /// <param name="us">The item series item to read from.</param>
    internal static double GetWeight(this ITimeSeriesItem us)
    {
        return (double)us.AttValue["Weight"];
    }

    /// <summary>
    /// Set the weight of the time series item
    /// </summary>
    /// <param name="us">The item series item to set.</param>
    /// <param name="weight">The weight to assign to the time series item.</param>
    internal static void SetWeight(this ITimeSeriesItem us, double weight)
    {
        us.AttValue["Weight"] = weight;
    }

    /// <summary>
    /// Get the number of the time series.
    /// </summary>
    /// <param name="us">The time series to get the number for.</param>
    /// <returns>The number for the time series.</returns>
    internal static int GetNumber(this ITimeSeries us)
    {
        return (int)(double)us.AttValue["No"];
    }

    /// <summary>
    /// Get the name for the time series.
    /// </summary>
    /// <param name="us">The time series to get the number for.</param>
    /// <returns>The name of the time series.</returns>
    internal static string GetName(this ITimeSeries us)
    {
        return (string)us.AttValue["Name"];
    }

}
