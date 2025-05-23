﻿using System.Xml;

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
            // We need to make sure to clear out the network in case it has been altered if a future load happens.
            _previouslyLoaded = true;
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
    /// Holds if we have been loaded at least once.
    /// </summary>
    private bool _previouslyLoaded = false;

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
            if(_previouslyLoaded)
            {
                // If we already have a network open make sure to dispose of all of the network elements before trying
                // to load a new file.
                var net = _visum.Net;
                COM.ReleaseCOMObject(ref net);
            }
            try
            {
                _visum.LoadVersion(VersionFile);
                _previouslyLoaded = true;
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
        _lock.EnterReadLock();
        try
        {
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

    /// <summary>
    /// Run the given procedures without wiping out the general settings.
    /// Internal ONLY, you need to hold a write lock.
    /// </summary>
    /// <param name="fileName">The temporary file that we want to run.</param>
    internal void RunProceduresFromFileInternal(string fileName)
    {
        ObjectDisposedException.ThrowIf(_visum is null, this);
        _visum.Procedures.OpenXmlWithOptions(fileName, ReadFunctions: false, ResetFunctionsBeforeReading: false);
        _visum.Procedures.Execute();
    }


    /// <summary>
    /// Open a filter file
    /// MUST OWN THE WRITE LOCK
    /// </summary>
    /// <param name="filterFileName">The path of the filter file to loadd.</param>
    internal void OpenFilterInner(string filterFileName)
    {
        ObjectDisposedException.ThrowIf(_visum is null, this);
        _visum.Filters.Open(filterFileName);
    }

    /// <summary>
    /// Remove all filters
    /// MUST OWN THE WRITE LOCK
    /// </summary>
    internal void ResetAllFilters()
    {
        var filters = _visum!.Filters;
        filters.NodeFilter().UseFilter = false;
        filters.MainNodeFilter().UseFilter = false;
        filters.LinkFilter().UseFilter = false;
        filters.TurnFilter().UseFilter = false;
        var lineGroupFilter = filters.LineGroupFilter();
        lineGroupFilter.UseFilterForLineRouteItems = false;
        lineGroupFilter.UseFilterForLineRoutes = false;
        lineGroupFilter.UseFilterForLines = false;
        lineGroupFilter.UseFilterForVehJourneys = false;
        lineGroupFilter.UseFilterForVehJourneySections = false;
        lineGroupFilter.UseFilterForTimeProfiles = false;
        lineGroupFilter.UseFilterForTimeProfileItems = false;
    }

    #region IDispose

    private bool disposedValue;

    private void DisposeVisumInstance()
    {
        if (_visum is not null)
        {
            COM.ReleaseCOMObject(ref _visum);
        }
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                DisposeVisumInstance();
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
