using System.Xml;

namespace TMG.Visum.RoadAssignment;

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
    internal abstract void WriteParameters(XmlWriter writer);

    /// <summary>
    /// Called to check the parameters for the algorithm. Default implementation does nothing.
    /// </summary>
    internal virtual void CheckParameters() { }

}
