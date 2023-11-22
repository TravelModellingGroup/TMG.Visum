using VISUMLIB;

namespace TMG.Visum;

/// <summary>
/// Represents an instance within a Time Series.
/// </summary>
public class VisumTimeSeriesItem : IDisposable
{

    /// <summary>
    /// A reference to the underlying standard time series item
    /// </summary>
    private ITimeSeriesItem _timeSeriesItem;

    /// <summary>
    /// The instance that the standard time series belongs to.
    /// </summary>
    private VisumInstance _instance;

    internal VisumTimeSeriesItem(ITimeSeriesItem timeSeriesItem, VisumInstance instance)
    {
        _timeSeriesItem = timeSeriesItem;
        _instance = instance;
    }

    /// <summary>
    /// Cumulative share up to this distribution interval.
    /// </summary>
    public double AccumulatedShare
    {
        get => (double)_timeSeriesItem.AttValue["AccumShare"];
        set => _timeSeriesItem.AttValue["AccumShare"] = value;
    }

    /// <summary>
    /// Number of the matrix resulting from the specified matrix reference based on the current data.
    /// </summary>
    public int CurrentMatrixNumber
    {
        get => (int)_timeSeriesItem.AttValue["CurrentMatrixNo"];
    }

    /// <summary>
    /// Set the start and end times for the 
    /// </summary>
    /// <param name="startDate">The date the time series item should start at.</param>
    /// <param name="startTime">The time that the time series item should start at in seconds.</param>
    /// <param name="endDate">The date the time series item should end at.</param>
    /// <param name="endTime">The time that the time series item should end at in seconds.</param>
    public void SetTime(int? startDate, int startTime, int? endDate, int endTime)
    {
        // When we do this we can't have the start ever come before the 
        var originalStart = (int)(double)_timeSeriesItem.AttValue["StartTime"];
        // It is -1 because they start at 1
        var newStartTime = (Math.Max(startDate - 1 ?? 0, 0) * 24 * 60 * 60) + startTime;
        var newEndTime = (Math.Max((endDate - 1 ?? 0), 0) * 24 * 60 * 60) + endTime;
        if(newEndTime < newStartTime)
        {
            throw new VisumException("The start time must be less than or equal to the end time.");
        }
        if(newStartTime < originalStart || newEndTime < originalStart)
        {
            _timeSeriesItem.AttValue["StartTime"] = (double)newStartTime;
            _timeSeriesItem.AttValue["EndTime"] = (double)newEndTime;
        }
        else
        {
            _timeSeriesItem.AttValue["EndTime"] = (double)newEndTime;
            _timeSeriesItem.AttValue["StartTime"] = (double)newStartTime;
        }
    }

    /// <summary>
    /// Time of the start time of this time series entry.
    /// </summary>
    public TimeOnly StartTimeInDay
    {
        // Internally this is represented as time in seconds within the day
        get => TimeOnly.FromTimeSpan(TimeSpan.FromSeconds((double)_timeSeriesItem.AttValue["StartTimeInDay"]));
        private set => _timeSeriesItem.AttValue["StartTimeInDay"] = value.ToTimeSpan().TotalSeconds;
    }

    /// <summary>
    /// Time of the end time of this time series entry.
    /// </summary>
    public TimeOnly EndTimeInDay
    {
        // Internally this is represented as time in seconds within the day
        get => TimeOnly.FromTimeSpan(TimeSpan.FromSeconds((double)_timeSeriesItem.AttValue["EndTimeInDay"]));
        private set => _timeSeriesItem.AttValue["EndTimeInDay"] = value.ToTimeSpan().TotalSeconds;
    }

    /// <summary>
    /// Gets the date component of the Start Time
    /// </summary>
    public int? StartDay
    {
        get
        {
            try
            {
                var day = (int?)(double?)_timeSeriesItem.AttValue["StartDay"];
                return day;
            }
            catch
            {
                return null;
            }
        }
        private set
        {
            _timeSeriesItem.AttValue["StartDay"] = (double?)value;
        }
    }

    /// <summary>
    /// Gets the date component of the End Time
    /// </summary>
    public int? EndDay
    {
        get
        {
            try
            {
                var day = (int?)(double?)_timeSeriesItem.AttValue["EndDay"];
                return day;
            }
            catch
            {
                return null;
            }
        }
        private set
        {
            _timeSeriesItem.AttValue["EndDay"] = (double?)value;
        }
    }

    /// <summary>
    /// The name of the demand matrix
    /// </summary>
    public string MatrixLabel
    {
        get => (string)_timeSeriesItem.AttValue["MatrixLabel"];
    }

    /// <summary>
    /// The demand matrix for this time segment
    /// </summary>
    public VisumMatrix MatrixRef
    {
        get
        {
            if (_instance.Visum is not null)
            {
                return new VisumMatrix((IMatrix)_timeSeriesItem.AttValue["MatrixRef"], ObjectTypeRefT.OBJECTTYPEREF_ZONE, _instance.Visum);
            }
            throw new VisumException("The Visum instance was already disposed.");
        }
        set => _timeSeriesItem.AttValue["MatrixRef"] = value.Matrix;
    }

    /// <summary>
    /// 
    /// </summary>
    public double Share
    {
        get => (double)_timeSeriesItem.AttValue["Share"];
    }

    /// <summary>
    /// Number of the time series.
    /// </summary>
    public int TimeSeriesNumber
    {
        get => (int)_timeSeriesItem.AttValue["TimeSeriesNo"];
    }

    /// <summary>
    /// 
    /// </summary>
    public double Weight
    {
        get => (double)_timeSeriesItem.AttValue["Weight"];
        set => _timeSeriesItem.AttValue["Weight"] = value;
    }

    ~VisumTimeSeriesItem()
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
                COM.ReleaseCOMObject(ref _timeSeriesItem!, false);
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