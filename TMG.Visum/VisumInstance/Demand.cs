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
}
