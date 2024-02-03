using System.Runtime.InteropServices;

namespace TMG.Visum;

public partial class VisumInstance
{

    /// <summary>
    /// Get the boards by line.
    /// This method will overwrite ADDVAL1 for the Lines.
    /// </summary>
    /// <returns>
    ///     A list of the transit line names with the 
    ///     corresponding sum of passenger boardings.
    /// </returns>
    /// <exception cref="VisumException">Throws if there is an error connecting to the VISUM server.</exception>
    public List<(string lineName, float boardings)> GetBoardings()
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            var ret = new List<(string lineName, float boardings)>();
            ExecuteEditAttributeInternal(new EditAttributeParameters()
            {
                NetObjectType = "LINE",
                Formula = "[SUMACTIVE:LINEROUTES\\SUMACTIVE:LINEROUTEITEMS\\PASSBOARD(AP)]",
                OnlyActive = true,
                ResultAttributeName = "ADDVAL1"
            });
            foreach(ILine line in _visum.Net.Lines)
            {
                double boardings = 0.0;
                boardings = (double)line.AttValue["ADDVAL1"];
                ret.Add((line.GetName(), (float)boardings));
            }
            return ret;
        }
        catch(COMException e)
        {
            throw new VisumException(e);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

}
