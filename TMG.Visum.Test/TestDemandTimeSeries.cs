namespace TMG.Visum.Test;

[TestClass]
public class TestDemandTimeSeries
{
    [TestMethod]
    public void CreateDemandTimeSeries()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var timeSeries = instance.GetStandardTimeSeries(1);
        using var demandTimeSeries = instance.CreateDemandTimeSeries("code", "name", timeSeries);
        instance.SaveVersionFile(@"C:\Users\James\Documents\temp\Test.ver");
    }

    [TestMethod]
    public void GetDemandTimeSeriesTimes()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
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

    [TestMethod]
    public void DeleteDemandTimeSeriesUsingReference()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
        using var mode = instance.CreateMode("Car", transitSystem);
        using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
        using var demandTimeSeries = demandSegment.DemandTimeSeries;
        instance.RemoveDemandTimeSeries(demandTimeSeries);        
    }

    [TestMethod]
    public void DeleteDemandTimeSeriesByNumber()
    {
        int number;
        using var instance = new VisumInstance("TestNetwork.ver");
        using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
        using var mode = instance.CreateMode("Car", transitSystem);
        {
            using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
            using var demandTimeSeries = demandSegment.DemandTimeSeries;
            number = demandTimeSeries.Number;
        }
        instance.RemoveDemandTimeSeries(number);
    }

    [TestMethod]
    public void DeleteDemandTimeSeriesByCode()
    {
        const string code = "DemandSegment";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
        using var mode = instance.CreateMode("Car", transitSystem);
        {
            using var demandSegment = instance.CreateDemandSegment(code, mode);
            using var demandTimeSeries = demandSegment.DemandTimeSeries;
        }
        instance.RemoveDemandTimeSeries(code);
    }

}
