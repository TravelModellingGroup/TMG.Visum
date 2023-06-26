namespace TMG.Visum.Utilities;

public static class MatrixExtensions
{
    /// <summary>
    /// Get the name of the matrix.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns>The name of the matrix.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetName(this IMatrix matrix)
    {
        var name = matrix.AttValue["Name"] as string;
        return name!;
    }

    /// <summary>
    /// Set the name of the matrix.
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="name">The name to set it to.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetName(this IMatrix matrix, string name)
    {
        matrix.AttValue["Name"] = name;
    }

    /// <summary>
    /// Get the matrix number.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns>The matrix number to look at.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNumber(this IMatrix matrix)
    {
        // The attribute is stored as a double
        // However Visum uses it as an int, so we need to
        // cast twice.
        return (int)((double)matrix.AttValue["No"]);
    }

    /// <summary>
    /// Gets the 0-indexed lookup for this in the matrix list
    /// </summary>
    /// <param name="matrix">The matrix to lookup.</param>
    /// <returns>The 0-indexed lookup in the matrix list for this matrix.</returns>
    public static int GetIndex(this IMatrix matrix)
    {
        // Yes the attribute is called number, not Index
        return (int)matrix.AttValue["Number"] - 1;
    }
}
