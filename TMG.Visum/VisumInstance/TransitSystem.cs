namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Create a new transit system with the given name and mode type
    /// </summary>
    /// <param name="name">The name of the transit system.</param>
    /// <param name="modeType"></param>
    /// <returns></returns>
    public VisumTransitSystem CreateTransitSystem(string name, ModeType modeType)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new VisumException("A transit system requires a name that is not blank or just whitespace.");
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            var tSystem = _visum.Net.AddTSystem(name, GetSystemTypeName(modeType));
            return new VisumTransitSystem(tSystem, this);
        }
        catch (VisumException)
        {
            // Just rethrow if it was an internal error
            throw;
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

    private static string GetSystemTypeName(ModeType modeType)
    {
        return modeType switch
        {
            ModeType.Road => "PRT",
            ModeType.PublicTransit => "PUT",
            ModeType.ActiveTransit => "PUTWALK",
            //Note: The error message also says there is a PUTAUX but I don't know what that means yet.
            _ => throw new VisumException("Invalid ModeType for a network!"),
        };
    }
}
