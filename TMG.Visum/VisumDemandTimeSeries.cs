using VISUMLIB;

namespace TMG.Visum;

/// <summary>
/// Represents a DemandTimeSeries within the VISUM
/// model.
/// </summary>
public sealed class VisumDemandTimeSeries : IDisposable
{
    private IDemandTimeSeries _timeSeries;
    private VisumInstance? _instance;

    internal VisumDemandTimeSeries(IDemandTimeSeries timeSeries, VisumInstance instance)
    {
        _timeSeries = timeSeries;
        _instance = instance;
    }

    /// <summary>
    /// Short name of the demand time series.
    /// </summary>
    public string Code
    {
        get => (string)_timeSeries.AttValue["Code"];
        set => _timeSeries.AttValue["Code"] = value;
    }

    /// <summary>
    /// Name of the demand time series.
    /// </summary>
    public string Name
    {
        get => (string)_timeSeries.AttValue["Name"];
        set => _timeSeries.AttValue["Name"] = value;
    }

    /// <summary>
    /// Number of the demand time series.
    /// </summary>
    public int Number
    {
        get => (int)_timeSeries.AttValue["No"];
        set => _timeSeries.AttValue["No"] = value;
    }

    /// <summary>
    /// Number of the demand time series.
    /// </summary>
    public int StandardTimeSeriesNumber
    {
        get => (int)(double)_timeSeries.AttValue["TimeSeriesNo"];
        set => _timeSeries.AttValue["TimeSeriesNo"] = (double)value;
    }

    /// <summary>
    /// Get the Standard Time Series for the demand time series
    /// </summary>
    public VisumStandardTimeSeries StandardTimeSeries
    {
        get
        {
            var number = StandardTimeSeriesNumber;
            if (_instance.Visum is IVisum instance)
            {
                return _instance.GetStandardTimeSeries(number);
            }
            throw new VisumException("The Visum instance was already disposed.");
        }
        set
        {

        }
    }

    ~VisumDemandTimeSeries()
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
}