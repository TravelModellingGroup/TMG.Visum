namespace TMG.Visum.Delete;

[ModuleInformation(Description = "Invoke the VISUM Delete Assignment Results procedure.")]
public sealed class DeleteAssignmentResults : IVisumTool
{
    [RunParameter("Delete PrT Results", true, "Delete private transit results.")]
    public bool DeletePrTResults;

    [RunParameter("Delete PuT Results", true, "Delete public transit results.")]
    public bool DeletePuTResults;

    public void Execute(VisumInstance visumInstance)
    {
        visumInstance.ExecuteDeleteAssignmentResults(DeletePrTResults, DeletePuTResults);
    }

    public bool RuntimeValidation(ref string? error)
    {

        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new (50,150,50);

}
