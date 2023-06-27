namespace TMG.Visum;

/// <summary>
/// This class wraps Visum's Demand Segment object
/// providing ways to interact with it.
/// </summary>
public sealed class VisumDemandSegment : IDisposable
{

    /// <summary>
    /// The demand segment that we wrap.
    /// </summary>
    public IDemandSegment _segment;

    /// <summary>
    /// The instance of Visum that the segment belongs to.
    /// </summary>
    public VisumInstance _instance;

    /// <summary>
    /// Setup a wrapper around a demand segment
    /// </summary>
    /// <param name="segment">The segment to wrap.</param>
    /// <param name="instance">The Visum instance that the segment belongs to.</param>
    internal VisumDemandSegment(IDemandSegment segment, VisumInstance instance)
    {
        _segment = segment;
        _instance = instance;
    }

    /// <summary>
    /// Get or set the demand matrix that this semgnet represents.
    /// </summary>
    public VisumMatrix? DemandMatrix
    {
        get
        {
            if (_instance.Visum is IVisum instance)
            {
                return new VisumMatrix(_segment.get_ODMatrix(), ObjectTypeRefT.OBJECTTYPEREF_ZONE, instance);
            }
            throw new VisumException("The Visum instance was already disposed.");
        }
        set
        {
            _segment.set_ODMatrix(value?.Matrix ?? null);
        }
    }

    #region IDipose
    private bool disposedValue;

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                COM.ReleaseCOMObject(ref _segment!, false);
            }
            disposedValue = true;
        }
    }

    ~VisumDemandSegment()
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
