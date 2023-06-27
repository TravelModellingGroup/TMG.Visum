namespace TMG.Visum;

/// <summary>
/// 
/// </summary>
public sealed class VisumTransitSystem : IDisposable
{
    private ITSystem _system;
    private VisumInstance _instance;
    
    internal VisumTransitSystem(ITSystem system, VisumInstance instance)
    {
        _system = system;
        _instance = instance;
    }

    /// <summary>
    /// The name of the transit system
    /// </summary>
    public string Name
    {
        get
        {
            return (_system.AttValue["Name"] as string)!;
        }
        set
        {
            _system.AttValue["Code"] = value ?? string.Empty;
        }
    }

    /// <summary>
    /// The unique code for the transit system.
    /// </summary>
    public string Code
    {
        get
        {
            return (_system.AttValue["Code"] as string)!;
        }
    }

    /// <summary>
    /// Internal, get a reference to the wrapped transit system object
    /// </summary>
    internal ITSystem TSystem => _system;

    #region IDispose

    ~VisumTransitSystem()
    {
        DisposeInternal();
    }

    private void DisposeInternal()
    {
        if (_system is not null)
        {
            COM.ReleaseCOMObject(ref _system!, false);
        }
        _instance = null!;
    }

    public void Dispose()
    {
        DisposeInternal();
        GC.SuppressFinalize(this);
    }
    #endregion
}
