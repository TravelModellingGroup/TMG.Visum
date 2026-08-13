namespace TMG.Visum.Edit;

[Module(
    Description = "Executes an EditAttribute procedure on the current VISUM instance.",
    Name = "Edit Attribute",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Edit/EditAttribute.html"
    )]
public sealed class EditAttribute : BaseAction<VisumInstance>
{
    [Parameter(Name = "Formula", DefaultValue = "", Description = "The formula to apply.", Index = 0)]
    public IFunction<string> Formula = null!;

    [Parameter(Name = "Network Object Type", DefaultValue = "LINK", Description = "The name of the network object.", Index = 1)]
    public IFunction<string> NetObjectType = null!;

    [Parameter(Name = "Result Attribute Name", DefaultValue = "", Description = "The name of the attribute to store the result into.", Index = 2)]
    public IFunction<string> ResultAttributeName = null!;

    [Parameter(Name = "Only Active", DefaultValue = "false", Description = "Should we assign to all network objects of the type or only the active ones?", Index = 3)]
    public IFunction<bool> OnlyActive = null!;

    [SubModule(Name = "Filter File", Required = false, Description = "A filter file to load before running the edit attribute.", Index = 4)]
    public IFunction<string>? FilterFile;

    public override void Invoke(VisumInstance visumInstance)
    {
        try
        {
            var filterFile = FilterFile is not null ? Path.GetFullPath(FilterFile.Invoke()) : null;
            var formula = Formula.Invoke();
            var netObjectType = NetObjectType.Invoke();
            var resultAttributeName = ResultAttributeName.Invoke();
            var onlyActive = OnlyActive.Invoke();
            visumInstance.ExecuteEditAttribute(new EditAttributeParameters()
            {
                Formula = formula,
                NetObjectType = netObjectType,
                ResultAttributeName = resultAttributeName,
                OnlyActive = onlyActive,
            }, filterFile);
        }
        catch (VisumException e)
        {
            throw new XTMFRuntimeException(this, "Unable to Edit Attribute", e);
        }
    }

    override public bool RuntimeValidation(ref string? error)
    {
        var formula = Formula.Invoke();
        var netObjectType = NetObjectType.Invoke();
        var resultAttributeName = ResultAttributeName.Invoke();

        if (string.IsNullOrWhiteSpace(formula))
        {
            error = "The formula must be non-empty!";
            return false;
        }
        if (string.IsNullOrWhiteSpace(netObjectType))
        {
            error = "The Network Object Type must be non-empty!";
            return false;
        }
        if (string.IsNullOrWhiteSpace(resultAttributeName))
        {
            error = "The Result Attribute Name must be non-empty!";
            return false;
        }
        return true;
    }

}
