using System.Globalization;
using System.Xml;

namespace TMG.Visum.RoadAssignment;

/// <summary>
/// Equilibrium Road Assignment
/// </summary>
/// <see cref="https://cgi.ptvgroup.com/vision-help/VISUM_2023_ENG/Content/1_Benutzermodell_IV/1_5_Gleichgewichtsumlegung.htm"/>
public sealed class EquilibriumAssignment : RoadAssignmentAlgorithm
{
    public override string VariantName => "Equilibrium";

    internal override void InnerWriteParameters(XmlWriter writer, StabilityCriteria criteria)
    {
        writer.WriteStartElement("EQUILIBRIUMPARA");
        writer.WriteAttributeString("MAXITERATIONS", criteria.MaxIterations.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("USECURRENTSOLUTION", "0");
        writer.WriteAttributeString("USEEXTENDEDSTABILITYCRITERIA", "1");
    }
}
