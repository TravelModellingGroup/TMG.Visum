
namespace TMG.Visum;

/// <summary>
/// Provides access to a Standard Time Series
/// </summary>
public class VisumStandardTimeSeries : IDisposable
{

    /// <summary>
    /// A reference to the underlying standard time series
    /// </summary>
    private ITimeSeries _timeSeries;

    /// <summary>
    /// The instance that the standard time series belongs to.
    /// </summary>
    private VisumInstance _instance;

    internal VisumStandardTimeSeries(ITimeSeries timeSeries, VisumInstance instance)
    {
        _timeSeries = timeSeries;
        _instance = instance;
    }

    /// <summary>
    /// The name of the standard time series
    /// </summary>
    public string Name
    {
        get => (string)_timeSeries.AttValue["Name"];
        set => _timeSeries.AttValue["Name"] = value;
    }

    /// <summary>
    /// The number of the standard time series
    /// </summary>
    public int Number
    {
        get => (int)(double)_timeSeries.AttValue["No"];
        set => _timeSeries.AttValue["No"] = (double)value;
    }

    /// <summary>
    /// 0 = Time series of shares / percentage
    /// 1 = Time series of matrices
    /// </summary>
    public int Type
    {
        get => (int)_timeSeries.AttValue["Type"];
        set => _timeSeries.AttValue["Type"] = value;
    }

    /// <summary>
    /// Get the time series item for the given
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public VisumTimeSeriesItem this[int index]
    {
        get
        {
            int current = 0;
            // The key is a start time not the index in the series so we
            // can't just get it by key.
            foreach (ITimeSeriesItem item in _timeSeries.TimeSeriesItems)
            {
                if (current == index)
                {
                    return new VisumTimeSeriesItem(item, _instance);
                }
                current++;
            }
            throw new VisumException($"There was no Time Series Item at index {index}!");
        }
        
    }

    /// <summary>
    /// The number of time series items within the time series
    /// </summary>
    public int Count
    {
        get => _timeSeries.TimeSeriesItems.Count;
    }

    ~VisumStandardTimeSeries()
    {
        Dispose(false);
    }

    private bool disposedValue;

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                COM.ReleaseCOMObject(ref _timeSeries!, false);
                _instance = null!;
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// INTERNAL ONLY - Gets the wrapped object.
    /// </summary>
    /// <returns>The underlying object this wrapper holds.</returns>
    internal ITimeSeries GetWrappedObject()
    {
        return _timeSeries;
    }
}