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
}
