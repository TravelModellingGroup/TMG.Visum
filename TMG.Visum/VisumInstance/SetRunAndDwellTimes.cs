using TMG.Visum.TransitAssignment;
using VISUMLIB;

namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    public void ExecuteSetRunAndDwellTimes(SetRunAndDwellTimeParameters parameters)
    {
        _lock.EnterWriteLock();
        try
        {
            ExecuteSetRunAndDwellTimesInternal(parameters);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    internal void ExecuteSetRunAndDwellTimesInternal(SetRunAndDwellTimeParameters parameters)
    {
        string? tempFileName = null;
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            tempFileName = WriteProcedure((writer) =>
            {
                writer.WriteStartElement("OPERATION");
                writer.WriteAttributeString("ACTIVE", "1");
                writer.WriteAttributeString("NO", "1");
                writer.WriteAttributeString("OPERATIONTYPE", "PuTAssignment");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                // end OPERATION
                writer.WriteEndElement();
            });
            // Wipe out the previous procedures and run this.
            _visum.Procedures.OpenXmlWithOptions(tempFileName, ResetFunctionsBeforeReading: false);
            _visum.Procedures.Execute();
        }
        catch (VisumException)
        {
            // Just pass through VisumExceptions
            throw;
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            Files.SafeDelete(tempFileName);
        }
    }

}

/// <summary>
/// 
/// </summary>
public sealed class SetRunAndDwellTimeParameters
{
    /// <summary>
    /// 
    /// </summary>
    public bool AddValues { get; init; } = false;

    /// <summary>
    /// 
    /// </summary>
    public string ModeForUnusedTimeProfileItems { get; init; } = "ALLTIMEPROFILES";

    /// <summary>
    /// 
    /// </summary>
    public float RunTimeConstant { get; init; } = 0.0f;

    /// <summary>
    /// 
    /// </summary>
    public string RunTimeLinkAttrId { get; init; } = "TCUR_PRTSYS(C)";

    /// <summary>
    /// 
    /// </summary>
    public float RunTimeLinkFactor { get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public string RunTimeMethod { get; init; } = "FROMLINKATTR";

    /// <summary>
    /// 
    /// </summary>
    public bool RunTimeGuardOnlyActiveLinks { get; init; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool RunTimeRegardSysRouteOnlyIfVehicleCombFits{ get; init; } = false;

    /// <summary>
    /// 
    /// </summary>
    public bool RunTimeRegardTurnsAndMainTurns { get; init; } = false;

    /// <summary>
    /// 
    /// </summary>
    public string RunTimeRoundingMethod { get; init; } = "ROUNDINGTIME_1S";

    /// <summary>
    /// 
    /// </summary>
    public string RunTimeTimeProfileItemAttrId { get; init; } = "ADDVAL";

    /// <summary>
    /// 
    /// </summary>
    public float RunTimeTimeProfileItemFactor{ get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public string RunTimeTurnAttrId { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public float RunTimeTurnFactor{ get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public float StopTimeConstant { get; init; } = 0.0f;

    /// <summary>
    /// 
    /// </summary>
    public string StopTimeMethod { get; init; } = "FROMTPIATTR";

    /// <summary>
    /// 
    /// </summary>
    public string StopTimeRoundingMethod { get; init; } = "ROUNDINGTIME_1S";

    /// <summary>
    /// 
    /// </summary>
    public string StopTimeStopPointAttrId { get; init; } = "";

    /// <summary>
    /// 
    /// </summary>
    public bool StopTimeStopPointFactor { get; init; } = true;

    /// <summary>
    /// 
    /// </summary>
    public string StopTimeTimeProfileItemAttrId{ get; init; } = "ADDVAL";

    /// <summary>
    /// 
    /// </summary>
    public float StopTimeTimeProfileItemFactor { get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public bool UpdateRunTime { get; init; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool UpdateStopTime { get; init; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool OnlyActiveTimeProfileItems { get; init; } = true;

}
