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
    /// Get the code of the matrix.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns>The code of the matrix.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetCode(this IMatrix matrix)
    {
        var name = matrix.AttValue["Code"] as string;
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
    /// Set the code of the matrix.
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="code">The code to set it to.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetCode(this IMatrix matrix, string code)
    {
        matrix.AttValue["Code"] = code;
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetIndex(this IMatrix matrix)
    {
        // Yes the attribute is called number, not Index
        return (int)matrix.AttValue["Number"] - 1;
    }

    /// <summary>
    /// Gets the Data Source Type from the matrix.
    /// </summary>
    /// <param name="matrix">The matrix to get the data source type from.</param>
    /// <returns>The data source type for the matrix, defaults to data if no type was set.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DataSourceType GetDataSourceType(this IMatrix matrix)
    {
        var o = matrix.AttValue["DataSourceType"];
        return o is not null ? (DataSourceType)o : DataSourceType.DataSourceTypeData;
    }

    /// <summary>
    /// Set the data source type for this matrix.
    /// </summary>
    /// <param name="matrix">The matrix to set the type for.</param>
    /// <param name="type">The type to assign to it.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetDataSourceType(this IMatrix matrix, DataSourceType type)
    {
        matrix.AttValue["DataSourceType"] = type;
    }

    /// <summary>
    /// Get the matrix type.
    /// </summary>
    /// <param name="matrix">The matrix to get the type from.</param>
    /// <returns>The matrix type, defaults to external.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MatrixType GetMatrixType(this IMatrix matrix)
    {
        var o = matrix.AttValue["MatrixType"];
        return o is not null ? (MatrixType)o : MatrixType.MATRIXTYPE_EXTERNAL;
    }

    /// <summary>
    /// Sets the matrix type.
    /// </summary>
    /// <param name="matrix">The matrix to assign to.</param>
    /// <param name="type">The type to assign to the matrix.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetMatrixType(this IMatrix matrix, MatrixType type)
    {
        matrix.AttValue["MatrixType"] = type;
    }

}
