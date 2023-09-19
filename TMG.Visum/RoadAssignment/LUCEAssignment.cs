using System.Xml;

namespace TMG.Visum.RoadAssignment;

/// <summary>
/// Linear User Cost Equilibrium (LUCE)
/// </summary>
/// <see cref="https://cgi.ptvgroup.com/vision-help/VISUM_2023_ENG/Content/1_Benutzermodell_IV/1_5_Linear_User_Cost_Equilibrium__LUCE_.htm"/>
public sealed class LUCEAssignment : RoadAssignmentAlgorithm
{
    public override string VariantName => throw new NotImplementedException();

    internal override void InnerWriteParameters(XmlWriter writer, StabilityCriteria criteria)
    {
        throw new NotImplementedException();
    }
}
