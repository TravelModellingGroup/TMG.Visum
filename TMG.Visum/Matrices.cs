namespace TMG.Visum;

public partial class VisumInstance : IDisposable
{
    public void ImportMatrix(string filePath)
    {
        _lock.EnterWriteLock();
        try
        {

            throw new NotImplementedException();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void ExportMatrix(string filePath)
    {
        _lock.EnterReadLock();
        try
        {
            throw new NotImplementedException();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
}
