using System.Xml;

namespace TMG.Visum.TransitAssignment;

/// <summary>
/// Provides an interface to generalize the different types of transit assignment.
/// </summary>
public abstract class TransitAlgorithmParameters
{
    /// <summary>
    /// Get the name of the variant
    /// </summary>
    internal abstract string VariantName { get; }

    /// <summary>
    /// Write the parameters for the particular type of transit assignment.
    /// </summary>
    /// <param name="writer">The writer to emit to</param>
    /// <param name="losToGenerate">The LoS data to calculate.</param>
    internal abstract void Write(XmlWriter writer, IList<PutLoSTypes> losToGenerate);

}
