using VISUMLIB;

namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Create a new mode with the given code / name.
    /// </summary>
    /// <param name="code">The code and name that will be given to the mode.</param>
    /// <param name="system">The transit system that the mode will be apart of.</param>
    /// <returns>A new mode with the given code and transit system.</returns>
    public VisumMode CreateMode(string code, VisumTransitSystem system)
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            return new VisumMode(_visum.Net.AddMode(code, system.TSystem), this);
        }
        catch(Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Create a new mode with the given code / name.
    /// </summary>
    /// <param name="code">The code and name that will be given to the mode.</param>
    /// <param name="system">The transit system that the mode will be apart of.</param>
    /// <returns>A new mode with the given code and transit system.</returns>
    public VisumMode CreateMode(string code, IEnumerable<VisumTransitSystem> systems)
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            return new VisumMode(_visum.Net.AddMode(code, systems.Select( x=> x.TSystem).ToArray()), this);
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}
