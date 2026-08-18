namespace TMG.Visum.Utilities;

internal static class DemandSegmentExtensions
{
    public static string GetCode(object us) => (string)((dynamic)us).AttValue["Code"];

    public static string GetName(object us) => (string)((dynamic)us).AttValue["Name"];

    public static void SetName(object us, string name) => ((dynamic)us).AttValue["Name"] = name;

    internal static dynamic GetMode(object us) => ((dynamic)us).AttValue["Mode"];

    internal static void SetMode(object us, dynamic mode) => ((dynamic)us).AttValue["Mode"] = mode;

    public static double GetOccupancyRate(object us) => (double)((dynamic)us).AttValue["OccupancyRate"];

    public static void SetOccupancyRate(object us, double value) => ((dynamic)us).AttValue["OccupancyRate"] = value;
}
