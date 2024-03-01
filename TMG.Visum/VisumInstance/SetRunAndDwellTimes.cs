using System.Globalization;
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
        static string GetBoolString(bool value) => value ? "1" : "0";
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            tempFileName = WriteProcedure((writer) =>
            {
                writer.WriteStartElement("OPERATION");
                writer.WriteAttributeString("ACTIVE", "1");
                writer.WriteAttributeString("NO", "1");
                writer.WriteAttributeString("OPERATIONTYPE", "UpdateTravelTimes");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                writer.WriteStartElement("UPDATETIMEPROFILETRAVELTIMESPARA");
                writer.WriteAttributeString("ADDVALUES", GetBoolString(parameters.AddValues));
                writer.WriteAttributeString("MODEFORUNUSEDTIMEPROFILEITEMS", parameters.ModeForUnusedTimeProfileItems);

                writer.WriteAttributeString("ONLYACTIVETIMEPROFILEITEMS", GetBoolString(parameters.OnlyActiveTimeProfileItems));
                writer.WriteAttributeString("RUNTIMECONSTANT", parameters.RunTimeConstant.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("RUNTIMELINKATTRID", parameters.RunTimeLinkAttrId);
                writer.WriteAttributeString("RUNTIMELINKFACTOR", parameters.RunTimeLinkFactor.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("RUNTIMEMETHOD", parameters.RunTimeMethod);
                writer.WriteAttributeString("RUNTIMEREGARDONLYACTIVELINKS", GetBoolString(parameters.OnlyActiveTimeProfileItems));
                writer.WriteAttributeString("RUNTIMEREGARDSYSROUTEONLYIFVEHCOMBFITS", "0");
                writer.WriteAttributeString("RUNTIMEREGARDTURNSANDMAINTURNS", "0");
                writer.WriteAttributeString("RUNTIMEROUNDINGMETHOD", parameters.RunTimeRoundingMethod);
                writer.WriteAttributeString("RUNTIMETIMEPROFILEITEMATTRID", parameters.RunTimeTimeProfileItemAttrId);
                writer.WriteAttributeString("RUNTIMETIMEPROFILEITEMFACTOR", parameters.RunTimeTimeProfileItemFactor.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("RUNTIMETURNATTRID", parameters.RunTimeTurnAttrId);
                writer.WriteAttributeString("STOPTIMECONSTANT", parameters.RunTimeTurnFactor.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("STOPTIMEMETHOD", parameters.StopTimeMethod);
                writer.WriteAttributeString("STOPTIMEROUNDINGMETHOD", parameters.StopTimeRoundingMethod);
                writer.WriteAttributeString("STOPTIMESTOPPOINTATTRID", parameters.StopTimeStopPointAttrId);
                writer.WriteAttributeString("STOPTIMESTOPPOINTFACTOR", parameters.StopTimeStopPointFactor.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("STOPTIMETIMEPROFILEITEMATTRID", parameters.StopTimeTimeProfileItemAttrId);
                writer.WriteAttributeString("STOPTIMETIMEPROFILEITEMFACTOR", parameters.StopTimeTimeProfileItemFactor.ToString(CultureInfo.InvariantCulture));

                writer.WriteAttributeString("UPDATERUNTIME", GetBoolString(parameters.UpdateRunTime));
                writer.WriteAttributeString("UPDATESTOPTIME", GetBoolString(parameters.UpdateStopTime));
                //END UPDATETIMEPROFILETRAVELTIMESPARA
                writer.WriteEndElement();

                // end OPERATION
                writer.WriteEndElement();
            });
            // Wipe out the previous procedures and run this.
            RunProceduresFromFileInternal(tempFileName);
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
    public string RunTimeLinkAttrId { get; init; } = "";

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
    public bool RunTimeRegardSysRouteOnlyIfVehicleCombFits { get; init; } = false;

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
    public float RunTimeTimeProfileItemFactor { get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public string RunTimeTurnAttrId { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public float RunTimeTurnFactor { get; init; } = 1.0f;

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
    public string StopTimeTimeProfileItemAttrId { get; init; } = "ADDVAL";

    /// <summary>
    /// 
    /// </summary>
    public float StopTimeTimeProfileItemFactor { get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public bool UpdateRunTime { get; init; } = false;

    /// <summary>
    /// 
    /// </summary>
    public bool UpdateStopTime { get; init; } = false;

    /// <summary>
    /// 
    /// </summary>
    public bool OnlyActiveTimeProfileItems { get; init; } = true;

}
