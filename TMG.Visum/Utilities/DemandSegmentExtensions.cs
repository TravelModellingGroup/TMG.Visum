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
}
