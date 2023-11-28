namespace TMG.Visum.Edit;

[ModuleInformation(
    Description = "Executes an EditAttribute procedure on the current VISUM instance."
    )]
public sealed class EditAttribute : IVisumTool
{
    [RunParameter("Formula", "", "The formula to apply.")]
    public string Formula = null!;

    [RunParameter("Network Object Type", "LINK", "The name of the network object.")]
    public string NetObjectType = null!;

    [RunParameter("Result Attribute Name", "", "The name of the attribute to store the result into.")]
    public string ResultAttributeName = null!;

    [RunParameter("Only Active", false, "Should we assign to all network objects of the type or only the active ones?")]
    public bool OnlyActive;

    public void Execute(VisumInstance visumInstance)
    {
        visumInstance.ExecuteEditAttribute(new EditAttributeParameters()
        {
            Formula = Formula,
            NetObjectType = NetObjectType,
            ResultAttributeName = ResultAttributeName,
            OnlyActive = OnlyActive,
        });
    }

    public bool RuntimeValidation(ref string? error)
    {
        if(string.IsNullOrWhiteSpace(Formula))
        {
            error = "The formula must be non-empty!";
            return false;
        }
        if (string.IsNullOrWhiteSpace(NetObjectType))
        {
            error = "The Network Object Type must be non-empty!";
            return false;
        }
        if (string.IsNullOrWhiteSpace(ResultAttributeName))
        {
            error = "The Result Attribute Name must be non-empty!";
            return false;
        }
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);
}
