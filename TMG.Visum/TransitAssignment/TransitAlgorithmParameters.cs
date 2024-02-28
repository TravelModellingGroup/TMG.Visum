using System.Diagnostics.CodeAnalysis;
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
    /// The day that the assignment starts on, 1 indexed
    /// </summary>
    public abstract int AssignmentStartDayIndex { get; init; }

    /// <summary>
    /// The day that the assignment ends on, 1 indexed
    /// </summary>
    public abstract int AssignmentEndDayIndex { get; init; }

    /// <summary>
    /// The time of day that the assignment starts on.
    /// </summary>
    public abstract TimeOnly AssignmentStartTime { get; init; }

    /// <summary>
    /// The time of day that the assignment ends at.
    /// </summary>
    public abstract TimeOnly AssignmentEndTime { get; init; }

    /// <summary>
    /// Write the parameters for the particular type of transit assignment.
    /// </summary>
    /// <param name="writer">The writer to emit to</param>
    /// <param name="losToGenerate">The LoS data to calculate.</param>
    internal abstract void Write(XmlWriter writer, IList<PutLoSTypes> losToGenerate);

    /// <summary>
    /// Test the parameters to make sure they are valid.
    /// </summary>
    /// <param name="instance">The instance of VISUM to use.</param>
    /// <returns>True if everything is alright</returns>
    internal abstract bool Validate(VisumInstance instance, [NotNullWhen(false)] out string? error);

}
