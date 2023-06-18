namespace TMG.Visum.Utilities;

public static class MatrixExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetName(this IMatrix matrix)
    {
        var name = matrix.AttValue["Name"] as string;
        return name!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetName(this IMatrix matrix, string name)
    {
        matrix.AttValue["Name"] = name;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNumber(this IMatrix matrix)
    {
        return (int)matrix.AttValue["No"];
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
