namespace TMG.Visum.Utilities;

internal static class DemandSegmentExtensions
{
    /// <summary>
    /// Get the code for the demand segment.
    /// </summary>
    /// <param name="us"></param>
    /// <returns>The unique code for the demand segment.</returns>
    public static string GetCode(this IDemandSegment us)
    {
        return (string)us.AttValue["Code"];
    }

    /// <summary>
    /// Get the name for this demand segment
    /// </summary>
    /// <param name="us"></param>
    /// <returns></returns>
    public static string GetName(this IDemandSegment us)
    {
        return (string)us.AttValue["Name"];
    }

    /// <summary>
    /// Set the name for this Demand Segment
    /// </summary>
    /// <param name="us"></param>
    /// <param name="name">The name to set the demand segment to.</param>
    public static void SetName(this IDemandSegment us, string name)
    {
        us.AttValue["Name"] = name;
    }

    /// <summary>
    /// Get the mode associated with the Demand Segment
    /// </summary>
    /// <param name="us"></param>
    /// <returns>The mode associated with this demand segment.</returns>
    internal static IMode GetMode(this IDemandSegment us)
    {
        return (IMode)us.AttValue["Mode"];
    }

    /// <summary>
    /// Set the associated mode to the given mode.
    /// </summary>
    /// <param name="us">The demand segment to alter.</param>
    /// <param name="mode">The mode to assign to the demand segment.</param>
    internal static void SetMode(this IDemandSegment us, IMode mode)
    {
        us.AttValue["Mode"] = mode;
    }

    /// <summary>
    /// Gets the occupancy rate for the segment.
    /// </summary>
    /// <param name="us">The segment to look at.</param>
    /// <returns>Get the occupancy rate of the demand segment</returns>
    public static double GetOccupancyRate(this IDemandSegment us)
    {
        return (double)us.AttValue["OccupancyRate"];
    }

    /// <summary>
    /// Set the occupancy rate for the segment.
    /// </summary>
    /// <param name="us">The segment to set.</param>
    /// <param name="value">The occupancy rate to set.</param>
    public static void SetOccupancyRate(this IDemandSegment us, double value)
    {
        us.AttValue["OccupancyRate"] = value;
    }

}
