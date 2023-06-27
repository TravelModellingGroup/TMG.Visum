namespace TMG.Visum;

/// <summary>
/// Provides a wrapper around a Visum Mode
/// </summary>
public class VisumMode : IDisposable
{
    private IMode _mode;

    private VisumInstance _instance;

    internal VisumMode(IMode mode, VisumInstance instance)
    {
        _mode = mode;
        _instance = instance;
    }

    internal IMode Mode => _mode;

    /// <summary>
    /// The name of the mode
    /// </summary>
    public string Name
    {
        get
        {
            return (_mode.AttValue["Name"] as string)!;
        }
        set
        {
            _mode.AttValue["Code"] = value ?? string.Empty;
        }
    }

    /// <summary>
    /// The unique code for the mode.
    /// </summary>
    public string Code
    {
        get
        {
            return (_mode.AttValue["Code"] as string)!;
        }
    }

    #region IDispose

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                COM.ReleaseCOMObject(ref _mode!, false);
            }
            _instance = null!;
            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    ~VisumMode()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
