using System.Globalization;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMG.Visum.TransitAssignment;

/// <summary>
/// Route Choice parameters for PutAssignment
/// </summary>
public sealed class HeadwayImpedanceParameters : TransitAlgorithmParameters
{
    internal override string VariantName => "Headway-based";

    public float AccessTimeVal { get; init; } = 0;

    public string BoardingPenaltyPuTAttribute { get; init; } = string.Empty;

    public string BoardingPenaltyPuTAuxAttribute { get; init; } = string.Empty;

    public float EgressTimeVal { get; init; } = 0;

    public float FarePointVal { get; init; } = 0;

    public float InVehicleTimeVal { get; init; } = 1;

    public string InVehicleTimeWeightAttribute { get; init; } = string.Empty;

    public string MeanDelayAttribute { get; init; } = string.Empty;

    public float NumberOfTransfersValue { get; init; } = 0;

    /// <summary>
    /// TODO: Figure out what this is
    /// </summary>
    public string OriginsToPareaPenaltyAttribute { get; init; } = string.Empty;

    public string OriginTimeProfilePenaltyAttribute { get; init; } = string.Empty;

    public string OriginWaitTimesToPareaWeightAttribute { get; init; } = string.Empty;

    public string OriginWaitTimeTimeProfileWeightAttribute { get; init; } = string.Empty;

    /// <summary>
    /// The initial wait time beta for PJT
    /// </summary>
    public float OriginWaitTimeValue { get; init; } = 1.0f;

    public float PerceivedJourneyTimeValue { get; init; } = 1.0f;

    public float PublicTransitAuxiliaryTimeValue { get; init; } = 1.0f;

    public float TransferWaitTimeValue { get; init; } = 1.0f;

    public string TransferWaitTimeWeightAttribute { get; init; } = string.Empty;

    public bool UseFareModel { get; init; } = true;

    public float WalkTimeValue { get; init; } = 1.0f;

    override public int AssignmentStartDayIndex { get; init; } = 1;

    override public TimeOnly AssignmentStartTime { get; init; } = TimeOnly.Parse("00:00:00");

    override public int AssignmentEndDayIndex { get; init; } = 2;

    override public TimeOnly AssignmentEndTime { get; init; } = TimeOnly.Parse("00:00:00");

    public bool RemoveDominatedPaths { get; init; } = true;

    public float ShareLowerBounds { get; init; } = 0.05f;

    override internal void Write(XmlWriter writer, IList<PutLoSTypes> loSToGenerate)
    {
        writer.WriteStartElement("HEADWAYBASEDASSIGNMENTPARAMETERS");
        writer.WriteStartElement("HEADWAYBASEDBASEPARA");
        writer.WriteAttributeString("CALCULATEASSIGNMENT", "1");
        writer.WriteAttributeString("CALCULATESFFORPUTODPAIRLIST", "0");
        writer.WriteAttributeString("CALCULATESKIMMATRICES", "1");
        writer.WriteAttributeString("FROMDESTZONENO", "");
        writer.WriteAttributeString("HEADWAYCALCULATION", "ExpectedWaitTime");
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
        writer.WriteAttributeString("PASSENGERINFORMATIONTYPE", "None_ExpDistRib_HW");
        writer.WriteAttributeString("REMOVEDOMINATEDPATHS", RemoveDominatedPaths ? "1" : "0");
        writer.WriteAttributeString("SHARELOWERBOUNDABS", ShareLowerBounds.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("SHAREUPPERBOUNDREL", "1");
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

}
