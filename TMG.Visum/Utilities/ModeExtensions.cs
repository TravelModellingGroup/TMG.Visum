namespace TMG.Visum.Utilities;

/// <summary>
/// Provides extension methods for VISUM modes.
/// </summary>
public static class ModeExtensions
{
    /// <summary>
    /// Get the code of the mode.
    /// </summary>
    /// <param name="mode">The mode to get the code of.</param>
    /// <returns>The code of the mode.</returns>
    internal static string GetCode(this IMode mode)
    {
        return (string)mode.AttValue["code"];
    }

    /// <summary>
    /// Get the name of the mode.
    /// </summary>
    /// <param name="mode">The mode to get the name of.</param>
    /// <returns>The name of the mode.</returns>
    internal static string GetName(this IMode mode)
    {
        return (string)mode.AttValue["Name"];
    }

}
