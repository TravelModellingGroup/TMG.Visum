using System.Diagnostics.CodeAnalysis;

namespace TMG.Visum;

public partial class VisumInstance
{

    /// <summary>
    /// Create a new demand segment with the given code and mode.
    /// </summary>
    /// <param name="code">The code for the new demand segment.</param>
    /// <param name="mode">The mode to use for this demand segment.</param>
    /// <returns>A reference to the newly created demand segment.</returns>
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

    /// <summary>
    /// Get a reference to the demand segment with the given code.
    /// </summary>
    /// <param name="code">The code of the demand segment to get.</param>
    /// <returns>A reference to the Demand Segment.</returns>
    /// <exception cref="VisumException">Thrown if there is no Demand Segment with the given code.</exception>
    public VisumDemandSegment GetDemandSegment(string code)
    {
        if (!TryGetDemandSegment(code, out var ret))
        {
            throw new VisumException($"There is no demand segment with the code {code}!");
        }
        return ret;
    }

    /// <summary>
    /// Tries to get a reference to the demand segment with the given code.
    /// </summary>
    /// <param name="code">The code to search for.</param>
    /// <param name="visumDemandSegment">A reference to the demand segment with the code, if found, null otherwise.</param>
    /// <returns>True if the demand segment was found, false otherwise.</returns>
    public bool TryGetDemandSegment(string code, [NotNullWhen(true)] out VisumDemandSegment? visumDemandSegment)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            IDemandSegment? ret = null;
            foreach (IDemandSegment segment in _visum.Net.DemandSegments)
            {
                if (segment.GetCode() == code)
                {
                    ret = segment;
                    break;
                }
            }
            if (ret is null)
            {
                visumDemandSegment = null;
                return false;
            }
            visumDemandSegment = new VisumDemandSegment(ret, this);
            return true;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

}
