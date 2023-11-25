using System.Runtime.InteropServices;
using System.Xml;
using VISUMLIB;

namespace TMG.Visum;

/// <summary>
/// This provides the interface to access a particular instance of VISUM.
/// </summary>
public sealed partial class VisumInstance : IDisposable
{
    /// <summary>
    /// Our link to VISUM, initialized
    /// at the start and set to null when disposed.
    /// </summary>
    private IVisum? _visum;

    /// <summary>
    /// This lock is used to make sure that we don't
    /// load a new version file when trying to save.
    /// </summary>
    private readonly ReaderWriterLockSlim _lock = new();

    /// <summary>
    /// The version of VISUM to use.
    /// TODO: Make this change depending on the version of VISUM that is actually loaded.
    /// </summary>
    private const string VersionString = "2401";

    /// <summary>
    /// Initializes a new instead of VISUM.
    /// </summary>
    /// <param name="caller">The module that wants to create this new instance.</param>
    /// <exception cref="VisumException">Thrown if there is an error when creating the new VISUM instance.</exception>
    public VisumInstance()
    {
        try
        {
            _visum = new VISUMLIB.Visum();
            VersionFile = string.Empty;
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
    }

    /// <summary>
    /// Internal only, get the real VISUM instance.
    /// </summary>
    internal IVisum? Visum => _visum;

    /// <summary>
    /// Initializes a new instead of VISUM and immediately loads the given version file.
    /// </summary>
    /// <param name="caller">The module that wants to create this new instance.</param>
    /// <param name="versionFile">The version file to load.</param>
    /// <exception cref="VisumException">
    ///     Thrown if there is an error when 
    ///     creating the new VISUM instance or if there is an issue loading the version file.
    /// </exception>
    public VisumInstance(string versionFile)
    {
        VersionFile = Path.GetFullPath(versionFile);
        _visum = new VISUMLIB.Visum();
        LoadVersionFile(versionFile);
    }

    /// <summary>
    /// The file path to the version file.
    /// </summary>
    public string VersionFile { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="versionFile">The version file to load.</param>
    public void LoadVersionFile(string versionFile)
    {
        try
        {
            _lock.EnterWriteLock();
            ObjectDisposedException.ThrowIf(_visum is null, this);
            VersionFile = Path.GetFullPath(versionFile);
            try
            {
                _visum.LoadVersion(VersionFile);
            }
            catch (Exception ex)
            {
                throw new VisumException(ex);
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Save the currently loaded version file to the given path
    /// </summary>
    /// <param name="filePath">The path to save the VISUM instance to.</param>
    public void SaveVersionFile(string filePath)
    {
        if (String.IsNullOrEmpty(filePath))
        {
            throw new VisumException($"{filePath} is an invalid file path to save a version file to.");
        }
        try
        {
            _lock.EnterReadLock();
            ObjectDisposedException.ThrowIf(_visum is null, this);   
            // Since people can use this to build a network from scratch if they want to
            // there is no reason to fore them to loading it from disk previously.
            _visum.SaveVersion(Path.GetFullPath(filePath));
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Call this to start generating a file that will be
    /// used for executing a procedure.
    /// </summary>
    /// <param name="writeBody">The function that will write the operation tag</param>
    private static string WriteProcedure(Action<XmlWriter> writeBody)
    {
        var fileName = Path.GetTempFileName();
        try
        {
            using var writer = XmlWriter.Create(fileName);
            writer.WriteStartDocument();

            writer.WriteStartElement("PROCEDURES");
            writer.WriteAttributeString("VERSION", VersionString);
            writer.WriteStartElement("OPERATIONS");
            writeBody(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
        catch
        {
            Files.SafeDelete(fileName);
            throw;
        }
        return fileName;
    }

    #region IDispose

    private bool disposedValue;

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                COM.ReleaseCOMObject(ref _visum);
                _lock.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
