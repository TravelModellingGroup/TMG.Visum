using System.Diagnostics.CodeAnalysis;

namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Create a new DemandTimeSeries
    /// </summary>
    /// <returns>A reference to the newly created DemandTimeSeries.</returns>
    /// <param name="code">The short name for this demand time series.</param>
    /// <param name="name">The full name for this demand time series</param>
    /// <param name="connectTo">The Standard Time Series that this demand time series will make reference to.</param>
    public VisumDemandTimeSeries CreateDemandTimeSeries(string code, string name, VisumStandardTimeSeries connectTo)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(code, nameof(code));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            int maxNumber = 1;
            foreach (IDemandTimeSeries series in _visum.Net.DemandTimeSeriesCont)
            {
                maxNumber = Math.Max(maxNumber, (int)(double)series.AttValue["No"]);
            }
            var timeSeries = _visum.Net.AddDemandTimeSeries(maxNumber + 1, connectTo.Number);
            var ret = new VisumDemandTimeSeries(timeSeries, this); ;
            ret.Name = name;
            ret.Code = code;
            return ret;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a demand time series with the given code.
    /// </summary>
    /// <param name="code">The code to search for.</param>
    /// <returns>The first demand time series with the given code.</returns>
    /// <exception cref="VisumException">Thrown if there is no Demand Time Series with the code.</exception>
    public VisumDemandTimeSeries GetDemandTimeSeries(string code)
    {
        if (!TryGetDemandTimeSeries(code, out var ret))
        {
            ThrowDemandTimeSeriesNotFound(code);
        }
        return ret;
    }

    [DoesNotReturn]
    private static void ThrowDemandTimeSeriesNotFound(string code)
    {
        throw new VisumException($"Unable to find a Demand Time Series with the code {code}!");
    }

    /// <summary>
    /// Try to get the Demand Time Series with the given code.  Returns true if
    /// it was found, false otherwise.
    /// </summary>
    /// <param name="code">The code to search for.</param>
    /// <param name="retSeries">The returned demand time series.</param>
    /// <returns>True if the Demand Time Series was found, false otherwise.</returns>
    public bool TryGetDemandTimeSeries(string code, [NotNullWhen(true)] out VisumDemandTimeSeries? retSeries)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(code, nameof(code));

        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            foreach (IDemandTimeSeries series in _visum.Net.DemandTimeSeriesCont)
            {
                if (((string)series.AttValue["Code"]).Equals(code))
                {
                    retSeries = new VisumDemandTimeSeries(series, this);
                    return true;
                }
            }
            retSeries = null;
            return false;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Remove the demand time series
    /// </summary>
    /// <param name="toDelete">The demand time series to remove.</param>
    public void RemoveDemandTimeSeries(VisumDemandTimeSeries toDelete)
    {
        ArgumentNullException.ThrowIfNull(toDelete, nameof(toDelete));

        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            _visum.Net.RemoveDemandTimeSeries(toDelete.GetInnerTimeSeries());
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Remove the demand time series with the given code.
    /// </summary>
    /// <param name="code">The code of the demand time series to remove.</param>
    public bool RemoveDemandTimeSeries(string code)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(code, nameof(code));

        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            foreach (IDemandTimeSeries series in _visum.Net.DemandTimeSeriesCont)
            {
                if (((string)series.AttValue["Code"]).Equals(code))
                {
                    _visum.Net.RemoveDemandTimeSeries(series);
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
    /// Remove the demand time series with the given code.
    /// </summary>
    /// <param name="number">The demand series number to remove.</param>
    public bool RemoveDemandTimeSeries(int number)
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            foreach (IDemandTimeSeries series in _visum.Net.DemandTimeSeriesCont)
            {
                if (((int)(double)series.AttValue["No"]) == number)
                {
                    _visum.Net.RemoveDemandTimeSeries(series);
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
    /// Get a demand time series given the time series' number.
    /// </summary>
    /// <param name="demandTimeSeriesNumber">The demand time series' number to get.</param>
    /// <returns>A reference to the demand time series.</returns>
    /// <exception cref="VisumException">Thrown if there is no demand time series with the given number.</exception>
    public VisumDemandTimeSeries GetDemandTimeSeries(int demandTimeSeriesNumber)
    {
        _lock.EnterReadLock();
        try
        {
            return GetDemandTimeSeriesInternal(demandTimeSeriesNumber);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Get a demand time series given the time series' number.
    /// This is an INTERNAL only call and will skip grabbing a read lock.
    /// </summary>
    /// <param name="demandTimeSeriesNumber">The demand time series' number to get.</param>
    /// <returns>A reference to the demand time series.</returns>
    /// <exception cref="VisumException">Thrown if there is no demand time series with the given number.</exception>
    internal VisumDemandTimeSeries GetDemandTimeSeriesInternal(int demandTimeSeriesNumber)
    {
        ObjectDisposedException.ThrowIf(_visum is null, this);
        IDemandTimeSeries? ret = _visum.Net.DemandTimeSeriesCont.ItemByKey[demandTimeSeriesNumber];
        return ret is null
            ? throw new VisumException($"There is no demand time series with the number {demandTimeSeriesNumber}!")
            : new VisumDemandTimeSeries(ret, this);
    }

}
