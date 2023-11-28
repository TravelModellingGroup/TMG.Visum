namespace TMG.Visum.TransitAssignment;

/// <summary>
/// The parameters that define a Surface Transit Speed
/// Updating class.
/// </summary>
public sealed class STSUParameters
{
    /// <summary>
    /// The duration per boarding to add to dwell time, in seconds.
    /// </summary>
    public required float BoardingDuration { get; init; }

    /// <summary>
    /// The duration per alighting to add to dwell time, in seconds.
    /// </summary>
    public required float AlightingDuration { get; init; }

    /// <summary>
    /// The dwell time that is added if the vehicle stops.
    /// </summary>
    public required float StopDuration { get; init; }

    /// <summary>
    /// The speed to set to the segment relative to the
    /// auto speed on the link.
    /// </summary>
    public required float AutoCorrelation { get; init; }

    /// <summary>
    /// The default speed for EROW's unless overridden.
    /// </summary>
    public required float DefaultEROWSpeed { get; init; }



}
