namespace TMG.Visum;

internal interface IVisumTool : IModule
{
    public void Execute(VisumInstance visumInstance);
}
