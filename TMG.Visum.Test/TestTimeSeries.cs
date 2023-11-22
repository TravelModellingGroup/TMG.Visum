namespace TMG.Visum.Test;

[TestClass]
public class TestTimeSeries
{
    [TestMethod]
    public void GetTimeSeriesTimes()
    {
        using var instance = new VisumInstance("TestNetwork2.ver");
        using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
        using var mode = instance.CreateMode("Car", transitSystem);
        using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
        using var demandTimeSeries = demandSegment.DemandTimeSeries;
        using var standardTimeSeries = demandTimeSeries.StandardTimeSeries;
        Assert.AreEqual(1, standardTimeSeries.Count);
        var date = 1;
        using var item = standardTimeSeries[0];
        item.SetTime(date, 0, date + 1, 0);
        Assert.AreEqual(date, item.StartDay);
        Assert.AreEqual(date + 1, item.EndDay);
    }

}
