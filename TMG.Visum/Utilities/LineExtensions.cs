namespace TMG.Visum.Utilities;

internal static class LineExtensions
{
    /// <summary>
    /// Get the name for the transit line.
    /// </summary>
    /// <param name="line">The transit line to get the name for.</param>
    /// <returns>The name of the transit line.</returns>
    internal static string GetName(this ILine line)
    {
        return line.AttValue["Name"] as string ?? string.Empty;
    }

}
