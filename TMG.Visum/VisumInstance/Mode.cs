using System.Diagnostics.CodeAnalysis;
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
        catch (Exception ex)
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
            return new VisumMode(_visum.Net.AddMode(code, systems.Select(x => x.TSystem).ToArray()), this);
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

    /// <summary>
    /// Tries to get the mode associated with the given code.
    /// </summary>
    /// <param name="code">The code to lookup.</param>
    /// <param name="mode">The mode associated with the code, null if none was found.</param>
    /// <returns>True if the mode was found, false otherwise.</returns>
    public bool TryGetMode(string code, [NotNullWhen(true)] out VisumMode? mode)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            foreach(IMode m in _visum.Net.Modes)
            {
                if(code.Equals(m.GetCode()))
                {
                    mode = new VisumMode(m, this);
                    return true;
                }
            }
            mode = null;
            return false;
        }
        finally 
        { 
            _lock.ExitReadLock(); 
        }
    }
}
