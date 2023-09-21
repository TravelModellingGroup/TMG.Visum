using System.Globalization;
using System.Xml;

namespace TMG.Visum.RoadAssignment;

/// <summary>
/// Linear User Cost Equilibrium (LUCE)
/// </summary>
/// <see cref="https://cgi.ptvgroup.com/vision-help/VISUM_2023_ENG/Content/1_Benutzermodell_IV/1_5_Linear_User_Cost_Equilibrium__LUCE_.htm"/>
public sealed class LUCEAssignment : RoadAssignmentAlgorithm
{
    public override string VariantName => "LUCE";

    internal override void InnerWriteParameters(XmlWriter writer, StabilityCriteria criteria)
    {
        //<LUCEPARA MAXITERATIONS="100" MESHPROPORTIONALITYOPTIMIZATIONMODE="DONOTOPTIMIZE" NUMPARALLELTASKS="1" USECURRENTSOLUTION="0" USEEXTENDEDSTABILITYCRITERIA="0">
        writer.WriteStartElement("LUCEPARA");
        writer.WriteAttributeString("MAXITERATIONS", criteria.MaxIterations.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MESHPROPORTIONALITYOPTIMIZATIONMODE", "DONOTOPTIMIZE");
        writer.WriteAttributeString("NUMPARALLELTASKS", Environment.ProcessorCount.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("USECURRENTSOLUTION", "0");
    }
}
