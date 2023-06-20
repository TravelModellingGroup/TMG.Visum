namespace TMG.Visum;

[ModuleInformation(Description = "Allows to easilly chain together commands to a Visum instance.")]
public sealed class ExecuteVisumTools : ISelfContainedModule
{
    [SubModelInformation(Required = true, Description = "The instance to use for all of the contained tools.")]
    public IDataSource<VisumInstance> Instance = null!;

    [SubModelInformation(Required = false, Description = "The tools to execute in order.")]
    public IVisumTool[] Tools = null!;

    private int _currentTool;

    public void Start()
    {
        var instance = Instance.LoadInstance();
        for (int i = 0; i < Tools.Length; i++)
        {
            _currentTool = i;
            Tools[i].Execute(instance);
        }
        _currentTool = Tools.Length;
    }

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = string.Empty;

    public float Progress
    {
        get
        {
            if (Tools.Length <= 0
                || _currentTool == Tools.Length)
            {
                return 1f;
            }
            var increment = 1.0f / Tools.Length;
            return (_currentTool + MathF.Min(0, Tools[_currentTool].Progress)) * increment;
        }
    }

    public Tuple<byte, byte, byte> ProgressColour => new(50, 50, 50);

    public override string ToString()
    {
        return (Tools.Length <= 0 ? string.Empty : Tools[_currentTool].ToString()) ?? string.Empty;
    }
}
