﻿namespace TMG.Visum;

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
    /// Internal Only, a reference to the wraped demand segment.
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
                return new VisumMatrix(_segment.get_ODMatrix(), ObjectTypeRefT.OBJECTTYPEREF_ZONE, instance);
            }
            throw new VisumException("The Visum instance was already disposed.");
        }
        set
        {
            _segment.set_ODMatrix(value?.Matrix ?? null);
        }
    }

    /// <summary>
    /// Get the occupancy rate for the demand segment.
    /// </summary>
    public double OccupancyRate
    {
        get => (double)_segment.AttValue["OccupancyRate"];
        set => _segment.AttValue["OccupancyRate"] = value;
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

    public VisumDemandTimeSeries DemandTimeSeries
    {
        get
        {
            if (_instance.Visum is IVisum instance)
            {
                var description = _segment.GetDemandDescription();
                var timeSeriesNumber = (int)(double)description.AttValue["DemandTimeSeriesNo"];
                return _instance.GetDemandTimeSeries(timeSeriesNumber);
            }
            throw new VisumException("The Visum instance was already disposed.");
        }
        set
        {

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
