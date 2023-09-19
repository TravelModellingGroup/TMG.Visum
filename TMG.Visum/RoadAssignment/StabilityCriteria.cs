namespace TMG.Visum.RoadAssignment;

/// <summary>
/// This class contains all of the parameters to control the stability of
/// road assignments.
/// </summary>
public sealed class StabilityCriteria
{
    /// <summary>
    /// The maximum number of iterations that will be run, even if the assignment has not converged.
    /// </summary>
    public int MaxIterations { get; init; } = 100;

    /// <summary>
    /// 
    /// </summary>
    public float MaxRelativeDifferenceLinkImpedanceFraction { get; init; } = 0.98f;

    public float MaxRelativeDifferenceLinkVolumeFraction { get; init; } = 0.98f;

    public float MaxRelativeDifferenceTurnImpedanceFraction { get; init; } = 0.98f;

    public float MaxRelativeDifferenceTurnVolumeFraction { get; init; } = 0.98f;

    public float IgnoreVolumesSmallerThan { get; init; } = 0.0f;

    public float MaxGap { get; init; } = 0.0001f;

    public float MaxRelativeDifferenceLinkImpedance { get; init; } = 0.01f;

    public float MaxRelativeDifferenceLinkVolume { get; init; } = 0.01f;

    public float MaxRelativeDifferenceTurnImpedance { get; init; } = 0.01f;

    public float MaxRelativeDifferenceTurnVolume { get; init; } = 0.01f;

    /// <summary>
    /// Go through the parameters and throw an exception if something doesn't make sense.
    /// </summary>
    /// <exception cref="VisumException">Thrown if there is a parameter that violates conditions.</exception>
    public void CheckParameters()
    {
        ThrowIfLessThanOrEqualToZero(MaxIterations, nameof(MaxIterations));
        
        RequireBetween0And1(MaxRelativeDifferenceTurnVolumeFraction, nameof(MaxRelativeDifferenceTurnVolumeFraction));
        RequireBetween0And1(MaxRelativeDifferenceTurnVolume, nameof(MaxRelativeDifferenceTurnVolume));
        RequireBetween0And1(MaxRelativeDifferenceTurnImpedance, nameof(MaxRelativeDifferenceTurnImpedance));
        RequireBetween0And1(MaxRelativeDifferenceTurnImpedanceFraction, nameof(MaxRelativeDifferenceTurnImpedanceFraction));
        RequireBetween0And1(MaxRelativeDifferenceLinkVolume, nameof(MaxRelativeDifferenceLinkVolume));
        RequireBetween0And1(MaxRelativeDifferenceLinkVolumeFraction, nameof(MaxRelativeDifferenceLinkVolumeFraction));
        RequireBetween0And1(MaxRelativeDifferenceLinkImpedance, nameof(MaxRelativeDifferenceLinkImpedance));
        RequireBetween0And1(MaxRelativeDifferenceLinkImpedanceFraction, nameof(MaxRelativeDifferenceLinkImpedanceFraction));

        RequireBetween0And1(MaxGap, nameof(MaxGap));
        RequireBetween0And1(IgnoreVolumesSmallerThan, nameof(IgnoreVolumesSmallerThan));
    }

    private static void RequireBetween0And1(float value, string variableName)
    {
        if(value < 0 || value > 1)
        {
            throw new VisumException(variableName + " must be between 0 and 1!");
        }
    }

    private static void ThrowIfLessThanOrEqualToZero(float value, string variableName)
    {
        if (value <= 0)
        {
            throw new VisumException(variableName + " must be greater than or equal to zero!");
        }
    }
}
