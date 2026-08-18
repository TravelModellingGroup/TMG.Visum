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
            int[] ret = new int[(int)zoneContainer.Count];
            int pos = 0;
            foreach (dynamic zone in zoneContainer)
            {
                ret[pos++] = (int)(double)zone.AttValue["No"];
            }
            return ret;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Gets all of the zone numbers, x coordinates and y-coordinates
    /// </summary>
    /// <returns>An array of all of the zone numbers in order.</returns>
    public (int[] zoneNumber, float[] x, float[] y) GetZoneInformation()
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            var zoneContainer = _visum.Net.Zones;
            int count = (int)zoneContainer.Count;
            int[] zoneNumber = new int[count];
            float[] x = new float[count];
            float[] y = new float[count];
            int pos = 0;
            foreach (dynamic zone in zoneContainer)
            {
                zoneNumber[pos] = (int)(double)zone.AttValue["No"];
                x[pos] = (float)(double)zone.AttValue["XCoord"];
                y[pos] = (float)(double)zone.AttValue["YCoord"];
                pos++;
            }
            return (zoneNumber, x, y);
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
