namespace TMG.Visum;

/// <summary>
/// Common operations
/// </summary>
internal static class Utilities
{
    /// <summary>
    /// Get the visum instance or create a new one.
    /// </summary>
    /// <param name="source">The datasource containing the instance.</param>
    /// <returns>A visum instance to operate on.</returns>
    public static VisumInstance LoadInstance(this IDataSource<VisumInstance> source)
    {
        if (!source.Loaded)
        {
            source.LoadData();
        }
        return source.GiveData()!;
    }
}
