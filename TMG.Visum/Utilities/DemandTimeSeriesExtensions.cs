namespace TMG.Visum.Utilities;

internal static class DemandTimeSeriesExtensions
{
    internal static string GetName(object series) => (string)((dynamic)series).AttValue["Name"];

    internal static void SetName(object series, string name) => ((dynamic)series).AttValue["Name"] = name;

    internal static int GetCode(object series) => (int)(double)((dynamic)series).AttValue["Code"];

    internal static void SetCode(object series, string code) => ((dynamic)series).AttValue["Code"] = code;

    internal static int GetStandardTimeSeriesNo(object series) => (int)(double)((dynamic)series).AttValue["TimeSeriesNo"];

    internal static void SetStandardTimeSeriesNo(object series, int number) => ((dynamic)series).AttValue["TimeSeriesNo"] = number;
}
