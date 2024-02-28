using TMG.Functions;

namespace TMG.Visum;

[ModuleInformation(Description = "Allows to easilly chain together commands to a Visum instance.")]
public sealed class ExecuteVisumTools : ISelfContainedModule
{
    [SubModelInformation(Required = true, Description = "The instance to use for all of the contained tools.", Index = 0)]
    public IDataSource<VisumInstance> Instance = null!;

    [SubModelInformation(Required = false, Description = "The tools to execute in order.", Index = 1)]
    public IVisumTool[] Tools = null!;

    [SubModelInformation(Required = false, Description = "Optional location to save the instance to when the tools finish executing.", Index = 2)]
    public FileLocation? SaveTo;

    [RunParameter("Only Save Final Iteration", false, "Should we only save on the final iteration?")]
    public bool OnlySaveFinalIteration;

    private int _currentTool;

    public void Start()
    {
        var loaded = Instance.Loaded;
        var instance = Instance.LoadInstance();
        try
        {
            for (int i = 0; i < Tools.Length; i++)
            {
                _currentTool = i;
                Tools[i].Execute(instance);
            }
            _currentTool = Tools.Length;
        }
        finally
        {
            if (SaveTo is not null)
            {
                if (!OnlySaveFinalIteration || 
                    _iterativeModel!.CurrentIteration == _iterativeModel.TotalIterations - 1)
                {
                    instance.SaveVersionFile(SaveTo);
                }
            }
            // If we were the ones to load the tool, unload it.
            if (!loaded)
            {
                Instance.UnloadData();
            }
        }
    }

    private IConfiguration _config;
    private IIterativeModel? _iterativeModel;

    public ExecuteVisumTools(IConfiguration config)
    {
        _config = config;
    }

    public bool RuntimeValidation(ref string? error)
    {
        if (ModelSystemReflection.GetRootOfType(_config, typeof(IIterativeModel), this, out var mss))
        {
            _iterativeModel = (IIterativeModel)mss.Module;
        }
        if (OnlySaveFinalIteration && _iterativeModel is null)
        {
            error = "There was no ancestor that implements the IIterative interface!";
            return false;
        }
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
