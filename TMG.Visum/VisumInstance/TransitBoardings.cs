namespace TMG.Visum;

public partial class VisumInstance
{

    public List<(string lineName, float boardings)> GetBoardings()
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);

            var ret = new List<(string lineName, float boardings)>();
            double boardings = 0.0;
            foreach(ILine line in _visum.Net.Lines)
            {
                foreach(ILineRoute route in line.LineRoutes)
                {
                    foreach(ILineRouteItem item in route.LineRouteItems)
                    {
                        boardings += item.GetBoardings();
                    }
                }
                ret.Add((line.GetName(), (float)boardings));
            }

            return ret;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

}
