namespace TMG.Visum.Test;

[TestClass]
public class TestDemandSegments
{
    [TestMethod]
    public void CreateDemandSegment()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
            using var mode = instance.CreateMode("Car", transitSystem);
            using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void OccupancyRate()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
            using var mode = instance.CreateMode("Car", transitSystem);
            using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
            const double rate = 2.0;
            demandSegment.OccupancyRate = rate;
            Assert.AreEqual(rate, demandSegment.OccupancyRate);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void PrFacAH()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
            using var mode = instance.CreateMode("Car", transitSystem);
            using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
            const double rate = 2.0;
            demandSegment.PrFacAH = rate;
            Assert.AreEqual(rate, demandSegment.PrFacAH);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void PrFacAP()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
            using var mode = instance.CreateMode("Car", transitSystem);
            using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
            const double rate = 2.0;
            demandSegment.PrFacAP = rate;
            Assert.AreEqual(rate, demandSegment.PrFacAP);
        }
        finally
        {
            instance.Dispose();
        }
    }
}
