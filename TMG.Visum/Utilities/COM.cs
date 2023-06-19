using System.Runtime.InteropServices;

namespace TMG.Visum.Utilities;

/// <summary>
/// Provides logic for releasing the memory 
/// from a COM object which comes from VISUM.
/// </summary>
internal static class COM
{
    /// <summary>
    /// Shutdown the remote COM object.
    /// </summary>
    /// <param name="obj">The COM object to shutdown.</param>
    internal static void ReleaseCOMObject<T>(ref T? obj, bool forceGC = true)
    {
        if (obj is null)
        {
            return;
        }
        if (OperatingSystem.IsWindows())
        {
            Marshal.FinalReleaseComObject(obj);
        }
        obj = default;
        // We collect and wait twice to ensure that cycles don't cause issues
        if (forceGC)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
