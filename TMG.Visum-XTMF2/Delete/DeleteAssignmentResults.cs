namespace TMG.Visum.Delete;

[Module(
    Description = "Invoke the VISUM Delete Assignment Results procedure.",
    Name = "Delete Assignment Results",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Delete/DeleteAssignmentResults.html"
    )]
public sealed class DeleteAssignmentResults : BaseAction<VisumInstance>
{
    [Parameter(Name = "Delete PrT Results", DefaultValue = "true", Description = "Delete private transit results.", Index = 0)]
    public IFunction<bool> DeletePrTResults = null!;

    [Parameter(Name = "Delete PuT Results", DefaultValue = "true", Description = "Delete public transit results.", Index = 1)]
    public IFunction<bool> DeletePuTResults = null!;

    public override void Invoke(VisumInstance visumInstance)
    {
        var deletePrT = DeletePrTResults.Invoke();
        var deletePuT = DeletePuTResults.Invoke();
        visumInstance.ExecuteDeleteAssignmentResults(deletePrT, deletePuT);
    }
}
