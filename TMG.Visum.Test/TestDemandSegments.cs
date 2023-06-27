namespace TMG.Visum.Test;

[TestClass]
public class TestDemandSegments
{
    [TestMethod]
    public void CreateDemandSegment()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
        using var mode = instance.CreateMode("Car", transitSystem);
        using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
    }
}
