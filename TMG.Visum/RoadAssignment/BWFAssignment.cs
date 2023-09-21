using System.Globalization;
using System.Xml;

namespace TMG.Visum.RoadAssignment;

/// <summary>
/// Bi-conjugate Frank-Wolfe
/// </summary>
/// <see cref="https://cgi.ptvgroup.com/vision-help/VISUM_2023_ENG/Content/1_Benutzermodell_IV/1_5_Bi-Conjugate_Verfahren.htm"/>
public sealed class BWFAssignment : RoadAssignmentAlgorithm
{
    public override string VariantName => "FRANKWOLFE";

    internal override void InnerWriteParameters(XmlWriter writer, StabilityCriteria criteria)
    {
        //<FRANKWOLFEPARA CALCICADURINGASSIGNMENT="0" MAXITERATIONS="200" USEEXTENDEDSTABILITYCRITERIA="0">
        writer.WriteStartElement("FRANKWOLFEPARA");
        writer.WriteAttributeString("CALCICADURINGASSIGNMENT", "0");
        writer.WriteAttributeString("MAXITERATIONS", criteria.MaxIterations.ToString(CultureInfo.InvariantCulture));
    }

}
