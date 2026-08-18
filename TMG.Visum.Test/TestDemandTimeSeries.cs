namespace TMG.Visum.Test;

[TestClass]
public class TestDemandTimeSeries
{
    [TestMethod]
    public void CreateDemandTimeSeries()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var timeSeries = instance.GetStandardTimeSeries(1);
            using var demandTimeSeries = instance.CreateDemandTimeSeries("code", "name", timeSeries);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void GetDemandTimeSeriesTimes()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
            using var mode = instance.CreateMode("Car", transitSystem);
            using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
            using var demandTimeSeries = demandSegment.DemandTimeSeries;
            Assert.IsNotNull(demandTimeSeries);
            using var standardTimeSeries = demandTimeSeries.StandardTimeSeries;
            Assert.AreEqual(1, standardTimeSeries.Count);
            var date = 1;
            using var item = standardTimeSeries[0];
            item.SetTime(date, 0, date + 1, 0);
            Assert.AreEqual(date, item.StartDay);
            Assert.AreEqual(date + 1, item.EndDay);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void DeleteDemandTimeSeriesUsingReference()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
            using var mode = instance.CreateMode("Car", transitSystem);
            using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
            using var demandTimeSeries = demandSegment.DemandTimeSeries;
            Assert.IsNotNull(demandTimeSeries);
            instance.RemoveDemandTimeSeries(demandTimeSeries);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void DeleteDemandTimeSeriesByNumber()
    {
        int number;
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
            using var mode = instance.CreateMode("Car", transitSystem);
            {
                using var demandSegment = instance.CreateDemandSegment("DemandSegment", mode);
                using var demandTimeSeries = demandSegment.DemandTimeSeries;
                Assert.IsNotNull(demandTimeSeries);
                number = demandTimeSeries.Number;
            }
            instance.RemoveDemandTimeSeries(number);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void DeleteDemandTimeSeriesByCode()
    {
        const string code = "DemandSegment";
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSystem = instance.CreateTransitSystem("RoadNetwork", ModeType.Road);
            using var mode = instance.CreateMode("Car", transitSystem);
            {
                using var demandSegment = instance.CreateDemandSegment(code, mode);
                using var demandTimeSeries = demandSegment.DemandTimeSeries;
            }
            instance.RemoveDemandTimeSeries(code);
        }
        finally
        {
            instance.Dispose();
        }
    }

}
