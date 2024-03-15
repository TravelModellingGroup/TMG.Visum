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

    /// <summary>
    /// Apply active line filter
    /// </summary>
    /// <param name="filter">The VISUM filter to setup.</param>
    internal abstract void ApplyActiveLineFilter(ILineGroupFilter filter);

    /// <summary>
    /// Update the dwell times for this transit assignment.
    /// </summary>
    /// <param name="instance">The instance that this transit assignment will execute on.</param>
    internal abstract void UpdateDwellTimes(VisumInstance instance);

    /// <summary>
    /// Update the time profile speeds for lines using STSU.
    /// </summary>
    /// <param name="visumInstance">The instance that this transit assignment will execute on.</param>
    internal abstract void UpdateSTSUSegmentSpeeds(VisumInstance visumInstance);

    /// <summary>
    /// Called before running any iterations.
    /// </summary>
    /// <param name="visumInstance"></param>
    internal abstract void Setup(VisumInstance visumInstance);

    /// <summary>
    /// Called after all iterations have been completed.
    /// </summary>
    /// <param name="instance">The instance of VISUM that was used for the transit assignment.</param>
    internal abstract void CleanUp(VisumInstance instance);
}
