namespace TMG.Visum;

public partial class VisumInstance
{
    public VisumDemandSegment CreateDemandSegment(string code, VisumMode mode)
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            var segment = _visum.Net.AddDemandSegment(code, mode.Mode);
                
            return new VisumDemandSegment(segment, this);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public VisumDemandSegment GetDemandSegment(string code)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            IDemandSegment? ret = null;
            foreach(IDemandSegment segment in _visum.Net.DemandSegments)
            {
                if(segment.GetCode() == code)
                {
                    ret = segment;
                    break;
                }
            }
            if(ret is null)
            {
                throw new VisumException($"There is no demand segment with the code {code}!");
            }
            return new VisumDemandSegment(ret, this);
        }
        finally
        {
            _lock.ExitReadLock();
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
            ObjectDisposedException.ThrowIf(_visum is null, this);
            IDemandTimeSeries? ret = _visum.Net.DemandTimeSeriesCont.ItemByKey[demandTimeSeriesNumber];
            if (ret is null)
            {
                throw new VisumException($"There is no demand time series with the number {demandTimeSeriesNumber}!");
            }
            return new VisumDemandTimeSeries(ret, this);
        }
        finally
        {
            _lock.ExitReadLock();
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
}
