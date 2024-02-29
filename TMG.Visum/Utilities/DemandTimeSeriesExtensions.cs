namespace TMG.Visum.Utilities;

/// <summary>
/// Provides functions to clean-up access to
/// Demand Time Series objects.
/// </summary>
internal static class DemandTimeSeriesExtensions
{

    /// <summary>
    /// Gets the name of the time series
    /// </summary>
    /// <param name="series">The series to operate on.</param>
    /// <returns></returns>
    internal static string GetName(this IDemandTimeSeries series)
    {
        return (string)series.AttValue["Name"];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="series">The series to operate on.</param>
    /// <param name="name"></param>
    internal static void SetName(this IDemandTimeSeries series, string name) 
    {
        series.AttValue["Name"] = name;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="series">The series to operate on.</param>
    /// <returns></returns>
    internal static int GetCode(this IDemandTimeSeries series)
    {
        return (int)(double)series.AttValue["Code"];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="series">The series to operate on.</param>
    /// <param name="code"></param>
    internal static void SetCode(this IDemandTimeSeries series, string code)
    {
        series.AttValue["Code"] = code;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="series">The series to operate on.</param>
    /// <returns></returns>
    internal static int GetStandardTimeSeriesNo(this IDemandTimeSeries series)
    {
        return (int)(double)series.AttValue["TimeSeriesNo"];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="series">The series to operate on.</param>
    /// <param name="number">The TimeSeriesNo to set it to, must be valid.</param>
    internal static void SetStandardTimeSeriesNo(this IDemandTimeSeries series, int number)
    {
        series.AttValue["TimeSeriesNo"] = number;
    }

}
