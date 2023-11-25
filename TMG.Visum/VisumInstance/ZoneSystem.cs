namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Get a list of all of the zone numbers
    /// </summary>
    /// <returns>An array of all of the zone numbers in order.</returns>
    public int[] GetZoneNumbers()
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            var zoneContainer = _visum.Net.Zones;
            return zoneContainer.GetZoneNumbers();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Get the total number of zones in the network.
    /// </summary>
    /// <returns>The total number of zones.</returns>
    public int GetZoneCount()
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            var zoneContainer = _visum.Net.Zones;
            return zoneContainer.Count;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

}
