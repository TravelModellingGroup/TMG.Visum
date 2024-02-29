using CommunityToolkit.HighPerformance;
using System.Diagnostics.CodeAnalysis;
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
            return CreateStandardTimeSeriesInner(name, byMatrices);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// INTERNAL ONLY -- Must be called while holding a write lock!
    /// </summary>
    /// <param name="name">The name to assign to this time series</param>
    /// <param name="byMatrices">Should be use Matrix Time Series, true, or Time series by percentage, false.</param>
    /// <returns></returns>
    internal VisumStandardTimeSeries CreateStandardTimeSeriesInner(string name, bool byMatrices)
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
            return RemoveStandardTimeSeriesInternal(number);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// INTERNAL ONLY -- You must have a write lock before calling this!
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    internal bool RemoveStandardTimeSeriesInternal(int number)
    {
        ObjectDisposedException.ThrowIf(_visum is null, this);

        // Before we do this we need to remove references to it
        // from DemandTimeSeries otherwise it will cascade delete them
        foreach(IDemandTimeSeries series in _visum.Net.DemandTimeSeriesCont)
        {
            if(series.GetStandardTimeSeriesNo() == number)
            {
                // TODO: Double check that the assumption for 1 is valid
                series.SetStandardTimeSeriesNo(1);
            }
        }

        // Now that we don't have a demand time series referencing this we
        // can now remove the time series
        foreach (ITimeSeries series in _visum.Net.TimeSeriesCont)
        {
            if (number == series.GetNumber())
            {
                _visum.Net.RemoveTimeSeries(series);
                return true;
            }
        }
        return false;
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
                if (name.Equals(series.GetName()))
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
        if (TryGetStandardTimeSeries(timeSeriesNumber, out var ret))
        {
            return ret;
        }
        throw new VisumException($"There is no standard time series with the number {timeSeriesNumber}!");
    }

    /// <summary>
    /// Get a standard time series given the time series' number.
    /// </summary>
    /// <param name="timeSeriesNumber">The number of the standard time series to get.</param>
    /// <returns>A reference to the standard time series.</returns>
    /// <exception cref="VisumException">Thrown if there is no standard time series with the given number.</exception>
    internal VisumStandardTimeSeries GetStandardTimeSeriesInternal(int timeSeriesNumber)
    {
        if (TryGetStandardTimeSeriesInternal(timeSeriesNumber, out var ret))
        {
            return ret;
        }
        throw new VisumException($"There is no standard time series with the number {timeSeriesNumber}!");
    }

    /// <summary>
    /// Get a standard time series given the time series' number.
    /// </summary>
    /// <param name="timeSeriesNumber">The number of the standard time series to get.</param>
    /// <param name="series">The series with the given time series number, null if not found.</param>
    /// <returns>True if we found the time series, false otherwise.</returns>
    public bool TryGetStandardTimeSeries(int timeSeriesNumber, [NotNullWhen(true)] out VisumStandardTimeSeries? series)
    {
        _lock.EnterReadLock();
        try
        {
            return TryGetStandardTimeSeriesInternal(timeSeriesNumber, out series);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// INTERANL ONLY - Must have read lock.
    /// Get a standard time series given the time series' number.
    /// </summary>
    /// <param name="timeSeriesNumber">The number of the standard time series to get.</param>
    /// <param name="series">The series with the given time series number, null if not found.</param>
    /// <returns>True if we found the time series, false otherwise.</returns>
    internal bool TryGetStandardTimeSeriesInternal(int timeSeriesNumber, [NotNullWhen(true)] out VisumStandardTimeSeries? series)
    {
        ObjectDisposedException.ThrowIf(_visum is null, this);
        ITimeSeries? ret = _visum.Net.TimeSeriesCont.ItemByKey[timeSeriesNumber];
        if (ret is not null)
        {
            series = new VisumStandardTimeSeries(ret, this);
            return true;
        }
        series = null;
        return false;
    }

    /// <summary>
    /// Get a standard time series given the time series' name.
    /// </summary>
    /// <param name="name">The name of the standard time series to get.</param>
    /// <returns>A reference to the standard time series.</returns>
    /// <exception cref="VisumException">Thrown if there is no standard time series with the given name.</exception>
    public VisumStandardTimeSeries GetStandardTimeSeries(string name)
    {
        if (TryGetStandardTimeSeries(name, out var ret))
        {
            return ret;
        }
        throw new VisumException($"There is no standard time series with the name {name}!");
    }

    /// <summary>
    /// Get a standard time series given the time series' name.
    /// </summary>
    /// <param name="name">The name of the standard time series to find.</param>
    /// <param name="series">The series with the given time series name. null if not found.</param>
    /// <returns>True if we found the time series, false otherwise.</returns>
    public bool TryGetStandardTimeSeries(string name, [NotNullWhen(true)] out VisumStandardTimeSeries? series)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            foreach (ITimeSeries s in _visum.Net.TimeSeriesCont)
            {
                if (name.Equals(s.GetName()))
                {
                    series = new VisumStandardTimeSeries(s, this);
                    return true;
                }
            }
            series = null;
            return false;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

}
