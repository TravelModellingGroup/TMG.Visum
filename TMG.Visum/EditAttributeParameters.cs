namespace TMG.Visum;

/// <summary>
/// Contains the parameters required to execute an EditAttribute operation.
/// </summary>
public sealed class EditAttributeParameters
{
    /// <summary>
    /// The formula to assign to the result attribute.
    /// </summary>
    public required string Formula { get; init; }

    /// <summary>
    /// The type of network object that the result attribute belongs to.
    /// </summary>
    public required string NetObjectType { get; init; }

    /// <summary>
    /// The name of the result attribute.
    /// </summary>
    public required string ResultAttributeName { get; init; }

    /// <summary>
    /// Should this operation only affect active objects?
    /// </summary>
    public required bool OnlyActive { get; init; }

}
