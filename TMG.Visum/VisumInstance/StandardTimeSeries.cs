using CommunityToolkit.HighPerformance;
using System.Xml.Linq;
using VISUMLIB;

namespace TMG.Visum;

public partial class VisumInstance
{

    /// <summary>
    /// Create a new standard time series object.
    /// </summary>
    /// <param name="name">The name to assign to this time series</param>
    /// <param name="byMatrices">Should be use Matrix Time Series, true, or Time series by percentage, false.</param>
    /// <returns></returns>
    public VisumStandardTimeSeries CreateStandardTimeSeries(string name, bool byMatrices)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            int maxNumber = 1;
            foreach (ITimeSeries series in _visum.Net.TimeSeriesCont)
            {
                maxNumber = Math.Max(maxNumber, (int)(double)series.AttValue["no"]);
            }
            var timeSeries = _visum.Net.AddTimeSeries(maxNumber + 1, byMatrices ? TimeSeriesDomainType.DomainTypeMatrices
                : TimeSeriesDomainType.DomainTypeShares);
            var ret = new VisumStandardTimeSeries(timeSeries, this)
            {
                Name = name
            };
            return ret;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Delete the given standard time series
    /// </summary>
    /// <param name="timeSeries">The time series to delete.</param>
    public void RemoveStandardTimeSeries(VisumStandardTimeSeries timeSeries)
    {
        ArgumentNullException.ThrowIfNull(timeSeries, nameof(timeSeries));

        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);

            var wrappedObject = timeSeries.GetWrappedObject();
            _visum.Net.RemoveTimeSeries(wrappedObject);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Delete the standard time series with the given number.
    /// </summary>
    /// <param name="number">The number of the time series to delete</param>
    /// <returns>True if there was a time series with the given number that was deleted.</returns>
    public bool RemoveStandardTimeSeries(int number)
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);

            foreach (ITimeSeries series in _visum.Net.TimeSeriesCont)
            {
                if(number == (int)(double)series.AttValue["no"])
                {
                    _visum.Net.RemoveTimeSeries(series);
                    return true;
                }
            }
            return false;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Delete the standard time series with the given name.
    /// </summary>
    /// <param name="name">The name of the time series to delete</param>
    /// <returns>True if there was a time series with the given name that was deleted.</returns>
    public bool RemoveStandardTimeSeries(string name)
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);

            foreach (ITimeSeries series in _visum.Net.TimeSeriesCont)
            {
                if (name.Equals((string)series.AttValue["Name"]))
                {
                    _visum.Net.RemoveTimeSeries(series);
                    return true;
                }
            }
            return false;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a standard time series given the time series' number.
    /// </summary>
    /// <param name="timeSeriesNumber">The number of the standard time series to get.</param>
    /// <returns>A reference to the standard time series.</returns>
    /// <exception cref="VisumException">Thrown if there is no standard time series with the given number.</exception>
    public VisumStandardTimeSeries GetStandardTimeSeries(int timeSeriesNumber)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            ITimeSeries? ret = _visum.Net.TimeSeriesCont.ItemByKey[timeSeriesNumber];
            if (ret is null)
            {
                throw new VisumException($"There is no standard time series with the number {timeSeriesNumber}!");
            }
            return new VisumStandardTimeSeries(ret, this);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Get a standard time series given the time series' name.
    /// </summary>
    /// <param name="name">The name of the standard time series to get.</param>
    /// <returns>A reference to the standard time series.</returns>
    /// <exception cref="VisumException">Thrown if there is no standard time series with the given name.</exception>
    public VisumStandardTimeSeries GetStandardTimeSeries(string name)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            foreach(ITimeSeries series in _visum.Net.TimeSeriesCont)
            {
                if (name.Equals((string)series.AttValue["Name"]))
                {
                    return new VisumStandardTimeSeries(series, this);
                }
            }
            throw new VisumException($"There is no standard time series with the name {name}!");
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

}
