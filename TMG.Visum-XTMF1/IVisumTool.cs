namespace TMG.Visum;

/// <summary>
/// Used for executing a set of tools against a single
/// Visum instance.
/// </summary>
public interface IVisumTool : IModule
{
    /// <summary>
    /// Called to execute the Visum tool.
    /// </summary>
    /// <param name="visumInstance">The Visum instance that should be used when executing.</param>
    public void Execute(VisumInstance visumInstance);
}
