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
    private IDemandSegment _segment;

    /// <summary>
    /// The instance of Visum that the segment belongs to.
    /// </summary>
    private VisumInstance _instance;

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
    /// Internal Only, a reference to the wrapped demand segment.
    /// </summary>
    internal IDemandSegment Segment => _segment;

    /// <summary>
    /// The unique code for the demand segment
    /// </summary>
    public string Code
    {
        get => _segment.GetCode();
    }

    /// <summary>
    /// Get and Set the Human readable name of the demand segment
    /// </summary>
    public string Name
    {
        get => _segment.GetName();
        set => _segment.SetName(value);
    }

    /// <summary>
    /// Get or set the demand matrix that this segment represents.
    /// </summary>
    public VisumMatrix? DemandMatrix
    {
        get
        {
            if (_instance.Visum is IVisum instance)
            {
                try
                {
                    var matrix = _segment.get_ODMatrix();
                    return new VisumMatrix(matrix, ObjectTypeRefT.OBJECTTYPEREF_ZONE, instance);
                }
                catch
                {
                    return null;
                }
            }
            throw new VisumException("The Visum instance was already disposed.");
        }
        set
        {
            _segment.set_ODMatrix(value?.Matrix);
        }
    }

    /// <summary>
    /// Get the occupancy rate for the demand segment.
    /// </summary>
    public double OccupancyRate
    {
        get => _segment.GetOccupancyRate();
        set => _segment.SetOccupancyRate(value);
    }

    /// <summary>
    /// Projection factor to project demand values from assignment period to analysis horizon.
    /// </summary>
    public double PrFacAH
    {
        get => (double)_segment.AttValue["PrFacAH"];
        set => _segment.AttValue["PrFacAH"] = value;
    }

    /// <summary>
    /// Projection factor to project demand values from assignment period to analysis period.
    /// </summary>
    public double PrFacAP
    {
        get => (double)_segment.AttValue["PrFacAP"];
        set => _segment.AttValue["PrFacAP"] = value;
    }

    public VisumDemandTimeSeries? DemandTimeSeries
    {
        get
        {
            ObjectDisposedException.ThrowIf(_instance.Visum is null, this);
            var description = _segment.GetDemandDescription();
            var timeSeriesNumber = (int)(double)description.AttValue["DemandTimeSeriesNo"];
            return _instance.GetDemandTimeSeries(timeSeriesNumber);
        }
        set
        {
            ObjectDisposedException.ThrowIf(_instance.Visum is null, this);
            var description = _segment.GetDemandDescription();
            description.AttValue["DemandTimeSeriesNo"] = (double)value.Number;
        }
    }

    /// <summary>
    /// INERNAL ONLY - You must have a read or write lock before calling this!
    /// </summary>
    /// <returns>The DemandTimeSeries for this demand segment.</returns>
    internal VisumDemandTimeSeries? GetDemandTimeSeriesInternal()
    {
        ObjectDisposedException.ThrowIf(_instance.Visum is null, this);
        var description = _segment.GetDemandDescription();
        var timeSeriesNumber = (int)(double)description.AttValue["DemandTimeSeriesNo"];
        return timeSeriesNumber == 0 ?
                  null
                : _instance.GetDemandTimeSeriesInternal(timeSeriesNumber);
    }

    /// <summary>
    /// Get or set the associated mode for this demand segment.
    /// </summary>
    public VisumMode Mode
    {
        get
        {
            ObjectDisposedException.ThrowIf(_instance.Visum is null, this);
            return new VisumMode(_segment.GetMode(), _instance);
        }
        set
        {
            _segment.SetMode(value.Mode);
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
            _instance = null!;
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
