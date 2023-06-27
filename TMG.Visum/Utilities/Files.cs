namespace TMG.Visum.Utilities;

/// <summary>
/// Provides utility functions when dealing with files.
/// </summary>
internal static class Files
{
    /// <summary>
    /// Try to delete the file with the given path.
    /// If the path is null, or the file is not deletable
    /// no exception will be thrown.
    /// </summary>
    /// <param name="fileName">The name of the file to delete.</param>
    internal static void SafeDelete(string? fileName)
    {
        if (fileName is null)
        {
            return;
        }
        try
        {
            File.Delete(fileName);
        }
        catch 
        { 
        }
    }
}
