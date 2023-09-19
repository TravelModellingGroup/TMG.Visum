using System.Xml;

namespace TMG.Visum.RoadAssignment;

/// <summary>
/// Bi-conjugate Frank-Wolfe
/// </summary>
/// <see cref="https://cgi.ptvgroup.com/vision-help/VISUM_2023_ENG/Content/1_Benutzermodell_IV/1_5_Bi-Conjugate_Verfahren.htm"/>
public sealed class BWFAssignment : RoadAssignmentAlgorithm
{
    public override string VariantName => throw new NotImplementedException();

    internal override void InnerWriteParameters(XmlWriter writer, StabilityCriteria criteria)
    {
        throw new NotImplementedException();
    }
}
