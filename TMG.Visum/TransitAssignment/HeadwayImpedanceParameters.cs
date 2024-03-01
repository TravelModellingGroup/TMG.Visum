using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;

namespace TMG.Visum.TransitAssignment;

/// <summary>
/// Route Choice parameters for PutAssignment
/// </summary>
public sealed class HeadwayImpedanceParameters : TransitAlgorithmParameters
{
    internal override string VariantName => "Headway-based";

    /// <summary>
    /// 
    /// </summary>
    public float AccessTimeVal { get; init; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public string BoardingPenaltyPuTAttribute { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string BoardingPenaltyPuTAuxAttribute { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public float EgressTimeVal { get; init; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public float FarePointVal { get; init; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public float InVehicleTimeVal { get; init; } = 1;

    /// <summary>
    /// 
    /// </summary>
    public string InVehicleTimeWeightAttribute { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string MeanDelayAttribute { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public float NumberOfTransfersValue { get; init; } = 0;

    /// <summary>
    /// TODO: Figure out what this is
    /// </summary>
    public string OriginsToPareaPenaltyAttribute { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string OriginTimeProfilePenaltyAttribute { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string OriginWaitTimesToPareaWeightAttribute { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string OriginWaitTimeTimeProfileWeightAttribute { get; init; } = string.Empty;

    /// <summary>
    /// The initial wait time beta for PJT
    /// </summary>
    public float OriginWaitTimeValue { get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public float PerceivedJourneyTimeValue { get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public float PublicTransitAuxiliaryTimeValue { get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public float TransferWaitTimeValue { get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public string TransferWaitTimeWeightAttribute { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public bool UseFareModel { get; init; } = true;

    /// <summary>
    /// 
    /// </summary>
    public float WalkTimeValue { get; init; } = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    override public int AssignmentStartDayIndex { get; init; } = 1;

    /// <summary>
    /// 
    /// </summary>
    override public TimeOnly AssignmentStartTime { get; init; } = TimeOnly.Parse("00:00:00");

    /// <summary>
    /// 
    /// </summary>
    override public int AssignmentEndDayIndex { get; init; } = 2;

    /// <summary>
    /// 
    /// </summary>
    override public TimeOnly AssignmentEndTime { get; init; } = TimeOnly.Parse("00:00:00");

    /// <summary>
    /// 
    /// </summary>
    public bool RemoveDominatedPaths { get; init; } = true;

    /// <summary>
    /// 
    /// </summary>
    public float ShareLowerBounds { get; init; } = 0.05f;

    /// <summary>
    /// 
    /// </summary>
    public float ShareUpperBounds { get; init; } = 0.99f;

    /// <summary>
    /// An attribute used for either saving headways to, or to read from.
    /// </summary>
    public string HeadwayAttribute { get; init; } = string.Empty;


    /// <summary>
    /// Use the headways stored in the HeadwayAttribute instead of computing it.
    /// </summary>
    public bool UseStoredHeadways { get; init; } = false;

    /// <summary>
    /// Use Optimized headway wait time, otherwise it will use constant wait times.
    /// </summary>
    public bool UseOptimizedHeadways { get; init; } = true;

    /// <summary>
    /// The parameters used for implementing STSU for the Headway Assignment.
    /// </summary>
    public STSUParameters[]? STSUParameters { get; init; } = null!;

    override internal void Write(XmlWriter writer, IList<PutLoSTypes> loSToGenerate)
    {
        if (UseStoredHeadways && string.IsNullOrWhiteSpace(HeadwayAttribute))
        {
            throw new VisumException("When using stored headways you must also have the headway attribute.");
        }

        writer.WriteStartElement("HEADWAYBASEDASSIGNMENTPARAMETERS");
        writer.WriteStartElement("HEADWAYBASEDBASEPARA");
        writer.WriteAttributeString("CALCULATEASSIGNMENT", "1");
        writer.WriteAttributeString("CALCULATESFFORPUTODPAIRLIST", "0");
        writer.WriteAttributeString("CALCULATESKIMMATRICES", "1");
        writer.WriteAttributeString("FROMDESTZONENO", "");
        writer.WriteAttributeString("HEADWAYCALCULATION", UseStoredHeadways ? "TimeProfileAttribute" : "ExpectedWaitTime");
        writer.WriteAttributeString("MPAISACTIVE", "0");
        writer.WriteAttributeString("SELECTODRELATIONTYPE", "All");
        //  TIMEINTERVALENDDAYINDEX="123" TIMEINTERVALENDTIME="00:00:00"  TIMEINTERVALSTARTDAYINDEX = "122" TIMEINTERVALSTARTTIME = "00:00:00"
        writer.WriteAttributeString("TIMEINTERVALENDDAYINDEX", AssignmentEndDayIndex.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("TIMEINTERVALENDTIME", AssignmentEndTime.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("TIMEINTERVALSTARTDAYINDEX", AssignmentStartDayIndex.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("TIMEINTERVALSTARTTIME", AssignmentStartTime.ToString(CultureInfo.InvariantCulture));

        writer.WriteStartElement("HEADWAYBASEDINTERVALDATA");
        writer.WriteAttributeString("TIMEINTERVALDAYINDEX", AssignmentStartDayIndex.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("TIMEINTERVALSTARTTIME", AssignmentStartTime.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("TODESTZONENO", AssignmentStartTime.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrWhiteSpace(HeadwayAttribute))
        {
            writer.WriteAttributeString("ATTRIBUTE", HeadwayAttribute);
        }
        // end HEADWAYBASEDINTERVALDATA
        writer.WriteEndElement();

        // end HEADWAYBASEDBASEPARA
        writer.WriteEndElement();

        // <HEADWAYBASEDDSEGPARA DSEGSETFORSKIMMATRICES=""/>
        writer.WriteStartElement("HEADWAYBASEDDSEGPARA");
        writer.WriteAttributeString("DSEGSETFORSKIMMATRICES", "");
        // END HEADWAYBASEDDSEGPARA
        writer.WriteEndElement();

        writer.WriteStartElement("HEADWAYBASEDSEARCHPARA");
        writer.WriteAttributeString("COORDINATEDTPSUNDISTINGUISHABLE", "1");
        writer.WriteAttributeString("COORDINATIONTYPE", "CoordByGroups");
        writer.WriteAttributeString("DISCRCHOICEAMONGSTOPAREASFACTOR", "0.25");
        writer.WriteAttributeString("DISCRCHOICEBETWEENBOARDANDALIGHTFACTOR", "0.25");
        writer.WriteAttributeString("INFOFORDIFFERENTSTOPAREAS", "0");
        writer.WriteAttributeString("INFOINVEHICLE", "0");
        writer.WriteAttributeString("INTERCHANGETYPE", "Use");
        writer.WriteAttributeString("MAXPOLYNOMDEGREEFORINTEGRATION", "30");
        writer.WriteAttributeString("NUMSIMULATIONS", "100");
        writer.WriteAttributeString("ONLYACTIVETIMEPROFILES", "1");
        writer.WriteAttributeString("PASSENGERINFORMATIONTYPE", UseOptimizedHeadways ? "None_ExpDistRib_HW" : "None_Constant_HW");
        writer.WriteAttributeString("REMOVEDOMINATEDPATHS", RemoveDominatedPaths ? "1" : "0");
        writer.WriteAttributeString("SHARELOWERBOUNDABS", ShareLowerBounds.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("SHAREUPPERBOUNDREL", ShareUpperBounds.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("USECALCULATIONTIMEOPTIMIZEDALGORITHM", "1");
        writer.WriteAttributeString("USEDISCRCHOICEAMONGSTOPAREAS", "0");
        writer.WriteAttributeString("USEDISCRCHOICEBETWEENBOARDANDALIGHT", "0");

        // end HEADWAYBASEDSEARCHPARA
        writer.WriteEndElement();

        // Write HEADWAYBASEDIMPEDANCEPARA
        writer.WriteStartElement("HEADWAYBASEDIMPEDANCEPARA");
        //<HEADWAYBASEDIMPEDANCEPARA
        writer.WriteAttributeString("ACCESSTIMEVAL", AccessTimeVal.ToString(CultureInfo.InvariantCulture));
        // ACCESSTIMEVAL="3"
        writer.WriteAttributeString("BOARDINGPENALTYPUTATTR", BoardingPenaltyPuTAttribute);
        // BOARDINGPENALTYPUTATTR=""
        writer.WriteAttributeString("BOARDINGPENALTYPUTAUXATTR", BoardingPenaltyPuTAuxAttribute);
        // BOARDINGPENALTYPUTAUXATTR=""
        writer.WriteAttributeString("EGRESSTIMEVAL", EgressTimeVal.ToString(CultureInfo.InvariantCulture));
        // EGRESSTIMEVAL="4"
        writer.WriteAttributeString("FAREPOINTVAL", FarePointVal.ToString(CultureInfo.InvariantCulture));
        // FAREPOINTVAL="13"
        writer.WriteAttributeString("INVEHTIMEVAL", InVehicleTimeVal.ToString(CultureInfo.InvariantCulture));
        // INVEHTIMEVAL="1"
        writer.WriteAttributeString("INVEHTIMEWEIGHTATTR", InVehicleTimeWeightAttribute);
        // INVEHTIMEWEIGHTATTR=""
        writer.WriteAttributeString("MEANDELAYATTR", MeanDelayAttribute);
        // MEANDELAYATTR=""
        writer.WriteAttributeString("NUMTRANSFERSVAL", NumberOfTransfersValue.ToString(CultureInfo.InvariantCulture));
        // NUMTRANSFERSVAL="11min"
        writer.WriteAttributeString("ORIGINSTOPAREAPENALTYATTR", OriginsToPareaPenaltyAttribute);
        // ORIGINSTOPAREAPENALTYATTR=""
        writer.WriteAttributeString("ORIGINTIMEPROFILEPENALTYATTR", OriginTimeProfilePenaltyAttribute);
        // ORIGINTIMEPROFILEPENALTYATTR=""
        writer.WriteAttributeString("ORIGINWAITTIMESTOPAREAWEIGHTATTR", OriginWaitTimesToPareaWeightAttribute);
        // ORIGINWAITTIMESTOPAREAWEIGHTATTR=""
        writer.WriteAttributeString("ORIGINWAITTIMETIMEPROFILEWEIGHTATTR", OriginWaitTimeTimeProfileWeightAttribute);
        // ORIGINWAITTIMETIMEPROFILEWEIGHTATTR=""
        writer.WriteAttributeString("ORIGINWAITTIMEVAL", OriginWaitTimeValue.ToString(CultureInfo.InvariantCulture));
        // ORIGINWAITTIMEVAL="6"
        writer.WriteAttributeString("PJTVAL", PerceivedJourneyTimeValue.ToString(CultureInfo.InvariantCulture));
        // PJTVAL="1"
        writer.WriteAttributeString("PUTAUXTIMEVAL", PublicTransitAuxiliaryTimeValue.ToString(CultureInfo.InvariantCulture));
        // PUTAUXTIMEVAL="2"
        writer.WriteAttributeString("TRANSFERWAITTIMEVAL", TransferWaitTimeValue.ToString(CultureInfo.InvariantCulture));
        // TRANSFERWAITTIMEVAL="7"
        writer.WriteAttributeString("TRANSFERWAITTIMEWEIGHTATTR", TransferWaitTimeWeightAttribute);
        // TRANSFERWAITTIMEWEIGHTATTR=""
        writer.WriteAttributeString("USEFAREMODEL", UseFareModel ? "1" : "0");
        // USEFAREMODEL="1"
        writer.WriteAttributeString("WALKTIMEVAL", WalkTimeValue.ToString(CultureInfo.InvariantCulture));
        // WALKTIMEVAL="5"/>
        // end HEADWAYBASEDIMPEDANCEPARA
        writer.WriteEndElement();

        writer.WriteStartElement("PUTSKIMMATRIXPARA");
        // CALCULATEANALYSISTIMEINTERVALSKIMS = "0" FILENAME = "" FORMAT = "V-Format" FUNCTION = "AggregateMean" LOWIMPCONNSHARE = "50" QUANTILE = "50" SELECTODRELATIONTYPE = "All" SEPARATOR = "Blank" VOLUMEWEIGHTED = "1"
        writer.WriteAttributeString("CALCULATEANALYSISTIMEINTERVALSKIMS", WalkTimeValue.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("FILENAME", "");
        writer.WriteAttributeString("FORMAT", "V-Format");
        writer.WriteAttributeString("FUNCTION", "AggregateMean");
        writer.WriteAttributeString("LOWIMPCONNSHARE", ShareLowerBounds.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("QUANTILE", "50");
        writer.WriteAttributeString("SELECTODRELATIONTYPE", "All");
        writer.WriteAttributeString("SEPARATOR", "Blank");
        writer.WriteAttributeString("VOLUMEWEIGHTED", "1");
        foreach (var matrix in loSToGenerate)
        {
            writer.WriteStartElement("SINGLESKIMMATRIXPARA");
            writer.WriteAttributeString("CALCULATE", "1");
            writer.WriteAttributeString("SAVETOFILE", "0");
            writer.WriteAttributeString("NAME", PuTLoSTypesHelper.GetCalculationName(matrix));
            // end SINGLESKIMMATRIXPARA
            writer.WriteEndElement();
        }

        // end PUTSKIMMATRIXPARA
        writer.WriteEndElement();

        // end HEADWAYBASEDASSIGNMENTPARAMETERS
        writer.WriteEndElement();

    }

    internal override bool Validate(VisumInstance instance, [NotNullWhen(false)] out string? error)
    {
        if (!string.IsNullOrWhiteSpace(HeadwayAttribute))
        {
            if (!instance.CheckAttributeExistsInternal(HeadwayAttribute, NetworkObjectType.TimeProfile))
            {
                error = $"The Headway Attribute '{HeadwayAttribute}' does not exist!";
                return false;
            }
        }
        if (string.IsNullOrWhiteSpace(HeadwayAttribute) && UseStoredHeadways)
        {
            error = $"You must specify the headway attribute if you are going to load in stored headways!";
            return false;
        }
        error = null;
        return true;
    }

    internal override void ApplyActiveLineFilter(ILineGroupFilter filter)
    {
        if (string.IsNullOrWhiteSpace(HeadwayAttribute) || !UseStoredHeadways)
        {
            return;
        }
        var timeProfileFilter = filter.TimeProfileFilter();
        timeProfileFilter.RemoveConditions();
        timeProfileFilter.AddCondition("OP_NONE", false, HeadwayAttribute, "LESSVAL", 9999.0);
        timeProfileFilter.UseFilter = true;

    }

    internal override void UpdateDwellTimes(VisumInstance instance)
    {
        if (STSUParameters is null)
        {
            return;
        }

        // Compute how long the assignment period is in hours
        var numberOfHours = 24.0 * (AssignmentEndDayIndex - AssignmentStartDayIndex) + (AssignmentEndTime - AssignmentStartTime).TotalHours;

        foreach (var parameter in STSUParameters)
        {
            // Update Filter
            instance.OpenFilterInner(parameter.FilterFileName);
            instance.SetLineGroupFilterInternal(true);
            // Setup Dwell Time
            instance.ExecuteEditAttributeInternal(new EditAttributeParameters()
            {
                NetObjectType = "TIMEPROFILEITEM",
                OnlyActive = true,
                ResultAttributeName = "ADDVAL",
                Formula = $"IF(MAX([ALIGHT],[BOARD]) > 0,{parameter.StopDuration} + ([LINEROUTEITEM\\PASSALIGHT(AP)] * {
                    parameter.AlightingDuration} + [LINEROUTEITEM\\PASSBOARD(AP)] * {parameter.BoardingDuration})*([{numberOfHours} * 60.0 / // {HeadwayAttribute}]),0)",
            });

            // Apply to lines in the filter
            instance.ExecuteSetRunAndDwellTimesInternal(new SetRunAndDwellTimeParameters()
            {
                AddValues = false,
                UpdateStopTime = true,
                StopTimeMethod = "FROMTPIATTR",
                StopTimeTimeProfileItemAttrId = "ADDVAL",
                StopTimeTimeProfileItemFactor = 1.0f,
                OnlyActiveTimeProfileItems = true,
            });
        }
    }

    internal override void UpdateSTSUSegmentSpeeds(VisumInstance instance)
    {
        if(STSUParameters is null)
        {
            return;
        }

        foreach (var parameter in STSUParameters)
        {
            // Update Filter
            instance.OpenFilterInner(parameter.FilterFileName);
            instance.SetLineGroupFilterInternal(true);
            var autoTimes = $"[LINEROUTEITEM\\OUTLINK\\TCUR_PRTSYS({parameter.AutoDemandSegment})]";
            // Setup Runtime
            instance.ExecuteEditAttributeInternal(new EditAttributeParameters()
            {
                NetObjectType = "TIMEPROFILEITEM",
                OnlyActive = true,
                ResultAttributeName = "ADDVAL",
                Formula = $"IF({autoTimes} < 9999, {autoTimes}, 60 * [LINEROUTEITEM\\OUTLINK\\LENGTH] / {parameter.DefaultEROWSpeed})"
            });

            // Apply to lines in the filter
            instance.ExecuteSetRunAndDwellTimesInternal(new SetRunAndDwellTimeParameters()
            {
                AddValues = false,
                UpdateRunTime = true,
                RunTimeTimeProfileItemFactor = parameter.AutoCorrelation,
                RunTimeTimeProfileItemAttrId = "ADDVAL",
                RunTimeMethod = "FROMTPIATTR",
                RunTimeGuardOnlyActiveLinks = true,
                OnlyActiveTimeProfileItems = true,

            });
        }
    }
}
