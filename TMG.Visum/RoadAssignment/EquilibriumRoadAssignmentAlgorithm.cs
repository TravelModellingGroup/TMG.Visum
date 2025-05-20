using System.Globalization;
using System.Xml;

namespace TMG.Visum.RoadAssignment;

/// <summary>
/// Provides the options for what type of
/// road assignments are supported by Visum.
/// </summary>
public abstract class EquilibriumRoadAssignmentAlgorithm(StabilityCriteria criteria) : RoadAssignmentAlgorithm
{
    /// <summary>
    /// The criteria for equilibrium stability for this assignment algorithm
    /// </summary>
    private readonly StabilityCriteria EqCriteria = criteria;

    /// <summary>
    /// Called to fill in the parameters for the XML execution script
    /// </summary>
    /// <param name="writer">The XML write to write the parameters to</param>
    /// <param name="criteria">
    /// The stability criteria to use for the road assignment.
    /// These must also be written out to file during this call.
    /// </param>
    internal override void WriteParameters(XmlWriter writer)
    {
        InnerWriteParameters(writer, EqCriteria);
        WriteStabilityCriteria(writer);
        // Close the algorithm element
        writer.WriteEndElement();
    }

    /// <summary>
    /// Checkes that the stability criteria parameters are correct; will throw
    /// exception if they are not.
    /// </summary>
    internal override void CheckParameters()
    {
        EqCriteria.CheckParameters();
    }

    /// <summary>
    /// Write the parameters that are specific to this road assignment.
    /// </summary>
    /// <param name="writer">The XML Write to use.</param>
    /// <param name="criteria">The stability criteria to use.</param>
    internal abstract void InnerWriteParameters(XmlWriter writer, StabilityCriteria criteria);

    /// <summary>
    /// Write out the common EXTENDEDSTABILITYCRITERIAPARA section
    /// for a road assignment
    /// </summary>
    /// <param name="criteria">The stopping criteria to use.</param>
    /// <param name="writer">The stream writer to store the values to.</param>
    protected void WriteStabilityCriteria(XmlWriter writer)
    {
        writer.WriteStartElement("EXTENDEDSTABILITYCRITERIAPARA");
        writer.WriteAttributeString("FRACTIONMAXRELDIFFLINKIMP", EqCriteria.MaxRelativeDifferenceLinkImpedanceFraction.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("FRACTIONMAXRELDIFFLINKVOL", EqCriteria.MaxRelativeDifferenceLinkVolumeFraction.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("FRACTIONMAXRELDIFFTURNIMP", EqCriteria.MaxRelativeDifferenceTurnImpedanceFraction.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("FRACTIONMAXRELDIFFTURNVOL", EqCriteria.MaxRelativeDifferenceTurnVolumeFraction.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("IGNOREVOLUMESMALLERTHAN", EqCriteria.IgnoreVolumesSmallerThan.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXGAP", EqCriteria.MaxGap.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXRELDIFFLINKIMP", EqCriteria.MaxRelativeDifferenceLinkImpedance.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXRELDIFFLINKVOL", EqCriteria.MaxRelativeDifferenceLinkVolume.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXRELDIFFTURNIMP", EqCriteria.MaxRelativeDifferenceTurnImpedance.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXRELDIFFTURNVOL", EqCriteria.MaxRelativeDifferenceTurnVolume.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("NUMCHECKEDSUBSEQUENTITERATIONS", "4");
        writer.WriteAttributeString("ONLYACTIVENETOBJECTS", "0");
        writer.WriteEndElement();
    }

}
