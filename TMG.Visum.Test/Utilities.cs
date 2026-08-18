using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TMG.Visum.Test;

internal static class Utilities
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
            ForceGC();
        }
    }

    internal static void ForceGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Marshal.CleanupUnusedObjectsInCurrentContext();
        while (Marshal.AreComObjectsAvailableForCleanup())
        {

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.CleanupUnusedObjectsInCurrentContext();
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
