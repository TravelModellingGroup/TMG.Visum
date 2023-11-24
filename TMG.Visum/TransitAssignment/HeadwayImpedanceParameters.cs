using System.Globalization;
using System.Xml;

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

    public int AssignmentStartDayIndex { get; init; } = 1;

    public TimeOnly AssignmentStartTime { get; init; } = TimeOnly.Parse("00:00:00");

    public int AssignmentEndDayIndex { get; init; } = 2;

    public TimeOnly AssignmentEndTime { get; init; } = TimeOnly.Parse("00:00:00");

    override internal void Write(XmlWriter writer, IList<PutLoSTypes> loSToGenerate)
    {
        writer.WriteStartElement("HEADWAYBASEDASSIGNMENTPARAMETERS");
        writer.WriteStartElement("HEADWAYBASEDBASEPARA");
        writer.WriteAttributeString("CALCULATEASSIGNMENT", "1");
        writer.WriteAttributeString("CALCULATESKIMMATRICES", "1");
        writer.WriteAttributeString("HEADWAYCALCULATION", "ExpectedWaitTime");
        writer.WriteAttributeString("SELECTODRELATIONTYPE", "All");
        //  TIMEINTERVALENDDAYINDEX="123" TIMEINTERVALENDTIME="00:00:00"  TIMEINTERVALSTARTDAYINDEX = "122" TIMEINTERVALSTARTTIME = "00:00:00"
        writer.WriteAttributeString("TIMEINTERVALENDDAYINDEX", AssignmentEndDayIndex.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("TIMEINTERVALENDTIME", AssignmentEndTime.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("TIMEINTERVALSTARTDAYINDEX", AssignmentStartDayIndex.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("TIMEINTERVALSTARTTIME", AssignmentStartTime.ToString(CultureInfo.InvariantCulture));

        writer.WriteStartElement("HEADWAYBASEDINTERVALDATA");
        writer.WriteAttributeString("TIMEINTERVALDAYINDEX", AssignmentStartDayIndex.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("TIMEINTERVALSTARTTIME", AssignmentEndDayIndex.ToString(CultureInfo.InvariantCulture));
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
        writer.WriteAttributeString("ONLYACTIVETIMEPROFILES", "0");
        writer.WriteAttributeString("PASSENGERINFORMATIONTYPE", "DeparturesFromStopArea");
        writer.WriteAttributeString("REMOVEDOMINATEDPATHS", "0");
        writer.WriteAttributeString("SHARELOWERBOUNDABS", "0.001");
        writer.WriteAttributeString("SHAREUPPERBOUNDREL", "1");
        writer.WriteAttributeString("USECALCULATIONTIMEOPTIMIZEDALGORITHM", "1");
        writer.WriteAttributeString("USEDISCRCHOICEAMONGSTOPAREAS", "1");
        writer.WriteAttributeString("USEDISCRCHOICEBETWEENBOARDANDALIGHT", "1");

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
        writer.WriteAttributeString("ORIGINWAITTIMEVAL", OriginWaitTimeTimeProfileWeightAttribute);
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
