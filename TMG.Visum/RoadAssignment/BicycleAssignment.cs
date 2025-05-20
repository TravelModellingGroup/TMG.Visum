using System.Xml;
using TMG.Visum;

namespace TMG.Visum.RoadAssignment;

public sealed class BicycleAssignment(List<VisumDemandSegment> demandSegments) : RoadAssignmentAlgorithm
{
    public enum BicycleChoiceModel
    {
        BoxCox,
        Kirchhoff,
        Logit,
        Lohse,
        LohseVariableBeta
    }


    public override string VariantName => "BICYCLE";

    public List<VisumDemandSegment> DemandSegments = demandSegments;

    public int NumSearchIterations { get; init; } = 10;

    public BicycleChoiceModel ChoiceModel { get; init; } = BicycleChoiceModel.Logit;

    public double Beta { get; init; } = 0.25;

    /*
    <BICYCLEASSIGNMENTPARA>
        <STOCHIMPEDANCEDSEGPARA DSEGCODE = "BIKE" GENERALPRTIMPEDANCEWEIGHT="1"/>
        <BICYCLESTOCHSEARCHPARA>
            <BICYCLESTOCHRANDOMPARA NUMSEARCHITER = "10" SIGMACOEFF="6" SIGMAIMPEXP="0.5"/>
            <STOCHDETOURTESTPARA CONSIDERROUTEIMPEDANCE = "0" MAXCOEFF="1.3" MAXDELTA="120s" PERFORMDETOURTEST="1"/>
        </BICYCLESTOCHSEARCHPARA>
        <BICYCLEPRESELECTIONPARA MAXEXTENDEDIMPCOEFF = "1.25" MAXEXTENDEDIMPDELTA="300" MAXIMPCOEFF="1.25" MAXIMPDELTA="300" MAXT0COEFF="1.25" MAXT0DELTA="300"/>
        <STOCHCHOICEPARA BOXCOXEXP = "1" BOXCOXPARA="0.5" CHOICEMODEL="Logit" IMPSCALINGDIVISOR="1" INDEPENDENCECALCEXACTLY="1" INDEPENDENCEVIAT0="0" KIRCHHOFFEXP="4" LOGITEXP="0.25" LOHSEEXP="4" LOHSEVARIABLEBETAKAPPA="10" LOHSEVARIABLEBETALAMBDA="0.8" LOHSEVARIABLEBETATAU="0.01"/>
        <STOCHIMPEDANCECOMMONPARA NUMEXTENDEDIMPVARIATIONS = "20" PERFORMEXTENDEDIMPVARIATION="1" SIGMAEXTENDEDIMPVARIATION="0.5"/>
        <STOCHSKIMPARA ONLYRELATIONSWITHDEMAND = "0" SELECTODRELATIONTYPE="All" SKIMCODE="IMPBICYCLE" WEIGHTING="Route Vol Avg">
            <ATTRIBUTE NAME = "CALCULATE" >
                <ATTRITEM SUBATTR1="ALLP" VALUE="0"/>
                <ATTRITEM SUBATTR1 = "BIKE" VALUE="0"/>
                <ATTRITEM SUBATTR1 = "CAR ADJ" VALUE="0"/>
                <ATTRITEM SUBATTR1 = "HGV" VALUE="0"/>
                <ATTRITEM SUBATTR1 = "MGV" VALUE="0"/>
                <ATTRITEM SUBATTR1 = "PNR" VALUE="0"/>
            </ATTRIBUTE>
        </STOCHSKIMPARA>
    </BICYCLEASSIGNMENTPARA>
    */

    internal override void WriteParameters(XmlWriter writer)
    {
        
        writer.WriteStartElement("BICYCLEASSIGNMENTPARA");

        foreach (VisumDemandSegment ds in DemandSegments)
        {
            writer.WriteStartElement("STOCHIMPEDANCEDSEGPARA");
            writer.WriteAttributeString("DSEGCODE", ds.Code);
            writer.WriteAttributeString("GENERALPRTIMPEDANCEWEIGHT", "1");
            writer.WriteEndElement();
        }

        writer.WriteStartElement("BICYCLESTOCHSEARCHPARA");

        writer.WriteStartElement("BICYCLESTOCHRANDOMPARA");
        writer.WriteAttributeString("NUMSEARCHITER", NumSearchIterations.ToString());
        writer.WriteAttributeString("SIGMACOEFF", "6");
        writer.WriteAttributeString("SIGMAIMPEXP", "0.5");
        writer.WriteEndElement();

        writer.WriteStartElement("STOCHDETOURTESTPARA");
        writer.WriteAttributeString("CONSIDERROUTEIMPEDANCE", "0");
        writer.WriteAttributeString("MAXCOEFF", "1.3");
        writer.WriteAttributeString("MAXDELTA", "120s");
        writer.WriteAttributeString("PERFORMDETOURTEST", "1");
        writer.WriteEndElement();

        writer.WriteEndElement();

        writer.WriteStartElement("BICYCLEPRESELECTIONPARA");
        writer.WriteAttributeString("MAXEXTENDEDIMPCOEFF", "1.25");
        writer.WriteAttributeString("MAXEXTENDEDIMPDELTA", "300");
        writer.WriteAttributeString("MAXIMPCOEFF", "1.25");
        writer.WriteAttributeString("MAXIMPDELTA", "300");
        writer.WriteAttributeString("MAXT0COEFF", "1.25");
        writer.WriteAttributeString("MAXT0DELTA", "300");
        writer.WriteEndElement();

        writer.WriteStartElement("STOCHCHOICEPARA");
        writer.WriteAttributeString("BOXCOXEXP", Beta.ToString());
        writer.WriteAttributeString("BOXCOXPARA", "0.5");
        writer.WriteAttributeString("CHOICEMODEL", Enum.GetName(ChoiceModel));
        writer.WriteAttributeString("IMPSCALINGDIVISOR", "1");
        writer.WriteAttributeString("INDEPENDENCECALCEXACTLY", "1");
        writer.WriteAttributeString("INDEPENDENCEVIAT0", "0");
        writer.WriteAttributeString("KIRCHHOFFEXP", "4");
        writer.WriteAttributeString("LOGITEXP", Beta.ToString());
        writer.WriteAttributeString("LOHSEEXP", "4");
        writer.WriteAttributeString("LOHSEVARIABLEBETAKAPPA", "10");
        writer.WriteAttributeString("LOHSEVARIABLEBETALAMBDA", "0.8");
        writer.WriteAttributeString("LOHSEVARIABLEBETATAU", "0.01");
        writer.WriteEndElement();

        writer.WriteStartElement("STOCHIMPEDANCECOMMONPARA");
        writer.WriteAttributeString("NUMEXTENDEDIMPVARIATIONS", "20");
        writer.WriteAttributeString("PERFORMEXTENDEDIMPVARIATION", "1");
        writer.WriteAttributeString("SIGMAEXTENDEDIMPVARIATION", "0.5");
        writer.WriteEndElement();

        writer.WriteStartElement("STOCHSKIMPARA");
        writer.WriteAttributeString("ONLYRELATIONSWITHDEMAND", "0");
        writer.WriteAttributeString("SELECTODRELATIONTYPE", "All");
        writer.WriteAttributeString("SKIMCODE", "IMPBICYCLE");
        writer.WriteAttributeString("WEIGHTING", "Route Vol Avg");

        writer.WriteStartElement("ATTRIBUTE");
        writer.WriteAttributeString("NAME", "CALCULATE");

        writer.WriteStartElement("ATTRITEM");
        writer.WriteAttributeString("SUBATTR1", "ALLP");
        writer.WriteAttributeString("VALUE", "0");
        writer.WriteEndElement();

        writer.WriteStartElement("ATTRITEM");
        writer.WriteAttributeString("SUBATTR1", "BIKE");
        writer.WriteAttributeString("VALUE", "0");
        writer.WriteEndElement();

        writer.WriteStartElement("ATTRITEM");
        writer.WriteAttributeString("SUBATTR1", "CAR ADJ");
        writer.WriteAttributeString("VALUE", "0");
        writer.WriteEndElement();

        writer.WriteStartElement("ATTRITEM");
        writer.WriteAttributeString("SUBATTR1", "HGV");
        writer.WriteAttributeString("VALUE", "0");
        writer.WriteEndElement();

        writer.WriteStartElement("ATTRITEM");
        writer.WriteAttributeString("SUBATTR1", "MGV");
        writer.WriteAttributeString("VALUE", "0");
        writer.WriteEndElement();

        writer.WriteStartElement("ATTRITEM");
        writer.WriteAttributeString("SUBATTR1", "PNR");
        writer.WriteAttributeString("VALUE", "0");
        writer.WriteEndElement();

        writer.WriteEndElement(); // ATTRIBUTE
        writer.WriteEndElement(); // STOCHSKIMPARA

        writer.WriteEndElement(); // BICYCLEASSIGNMENTPARA
    }



}
