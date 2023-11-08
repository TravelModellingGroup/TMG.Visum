using System.Globalization;
using System.Xml;
using TMG.Visum.RoadAssignment;

namespace TMG.Visum;

/// <summary>
/// Provides the options for what type of
/// road assignments are supported by Visum.
/// </summary>
public abstract class RoadAssignmentAlgorithm
{
    /// <summary>
    /// The name of the variant to use in the
    /// PRTASSIGNMENTVARIANT attribute.
    /// </summary>
    public abstract string VariantName { get; }

    /// <summary>
    /// Called to fill in the parameters for the XML execution script
    /// </summary>
    /// <param name="writer">The XML write to write the parameters to</param>
    /// <param name="criteria">
    /// The stability criteria to use for the road assignment.
    /// These must also be written out to file during this call.
    /// </param>
    internal void WriteParameters(XmlWriter writer, StabilityCriteria criteria)
    {
        InnerWriteParameters(writer, criteria);
        WriteStabilityCriteria(writer, criteria);
        // Close the algorithm element
        writer.WriteEndElement();
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
    protected static void WriteStabilityCriteria(XmlWriter writer, StabilityCriteria criteria)
    {
        writer.WriteStartElement("EXTENDEDSTABILITYCRITERIAPARA");
        writer.WriteAttributeString("FRACTIONMAXRELDIFFLINKIMP", criteria.MaxRelativeDifferenceLinkImpedanceFraction.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("FRACTIONMAXRELDIFFLINKVOL", criteria.MaxRelativeDifferenceLinkVolumeFraction.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("FRACTIONMAXRELDIFFTURNIMP", criteria.MaxRelativeDifferenceTurnImpedanceFraction.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("FRACTIONMAXRELDIFFTURNVOL", criteria.MaxRelativeDifferenceTurnVolumeFraction.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("IGNOREVOLUMESMALLERTHAN", criteria.IgnoreVolumesSmallerThan.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXGAP", criteria.MaxGap.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXRELDIFFLINKIMP", criteria.MaxRelativeDifferenceLinkImpedance.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXRELDIFFLINKVOL", criteria.MaxRelativeDifferenceLinkVolume.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXRELDIFFTURNIMP", criteria.MaxRelativeDifferenceTurnImpedance.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MAXRELDIFFTURNVOL", criteria.MaxRelativeDifferenceTurnVolume.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString("NUMCHECKEDSUBSEQUENTITERATIONS", "4");
        writer.WriteAttributeString("ONLYACTIVENETOBJECTS", "0");
        writer.WriteEndElement();
    }

}
