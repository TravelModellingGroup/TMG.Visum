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

    [TestMethod]
    public void OccupancyRate()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
        using var mode = instance.CreateMode("Car", transitSystem);
        using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
        const double rate = 2.0;
        demandSegment.OccupancyRate = rate;
        Assert.AreEqual(rate, demandSegment.OccupancyRate);
    }

    [TestMethod]
    public void PrFacAH()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
        using var mode = instance.CreateMode("Car", transitSystem);
        using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
        const double rate = 2.0;
        demandSegment.PrFacAH = rate;
        Assert.AreEqual(rate, demandSegment.PrFacAH);
    }

    [TestMethod]
    public void PrFacAP()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
        using var mode = instance.CreateMode("Car", transitSystem);
        using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
        const double rate = 2.0;
        demandSegment.PrFacAP = rate;
        Assert.AreEqual(rate, demandSegment.PrFacAP);
    }
}
