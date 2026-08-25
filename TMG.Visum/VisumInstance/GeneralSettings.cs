using System.Reflection;
using System.Runtime.InteropServices;

namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Load general settings from a file into the Visum instance.
    /// </summary>
    /// <param name="filePath"></param>
    public void LoadGeneralSettings(string filePath)
    {
        var fullPath = Path.GetFullPath(filePath);
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            var procedures = _visum.Procedures;
            try
            {
                // Use the XML-based open with explicit options. The Procedures COM object
                // exposes OpenXmlWithOptions (used elsewhere in this class) rather than Open,
                // so call that to avoid a runtime binding error.
                // ReadOperations = false
                // ReadFunctions = true (Controls whether general procedure settings are read)
                // AppendProcedures = false
                procedures.OpenXmlWithOptions(fullPath, ReadOperations: false, ReadFunctions: true, ResetFunctionsBeforeReading: true);
            }
            finally
            {
                // Release the COM reference we obtained from the Visum instance.
                TMG.Visum.Utilities.COM.ReleaseCOMObject(ref procedures, false);
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Save the current general settings of the Visum instance to a file.
    /// </summary>
    /// <param name="filePath">The location to save the general settings to.</param>
    public void SaveGeneralSettings(string filePath)
    {
        var fullPath = Path.GetFullPath(filePath);
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            // WriteOperations = false
            // WriteFunctions = true (Controls whether general procedure settings are read)
            // WriteVariables (there is no documentation for this option)
            _visum.Procedures.Save(fullPath, WriteOperations: false, WriteFunctions: true);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

}
