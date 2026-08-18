namespace TMG.Visum.Test;

[TestClass]
public class TestStandardTimeSeries
{

    [TestMethod]
    public void CreateStandardTimeSeries()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var newTimeSeries = instance.CreateStandardTimeSeries("NewTimeSeries", true);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void GetStandardTimeSeriesByNumber()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var timeSeries = instance.GetStandardTimeSeries(1);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void GetStandardTimeSeriesByName()
    {
        const string name = "NewTimeSeries";
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var newTimeSeries = instance.CreateStandardTimeSeries(name, true);
            using var foundTimeSeries = instance.GetStandardTimeSeries(name);
            Assert.AreEqual(newTimeSeries.Number, foundTimeSeries.Number);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void RemoveStandardTimeSeriesByReference()
    {
        const string name = "NewTimeSeries";
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var newTimeSeries = instance.CreateStandardTimeSeries(name, true);
            instance.RemoveStandardTimeSeries(newTimeSeries);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void RemoveStandardTimeSeriesByNumber()
    {
        int number;
        const string name = "NewTimeSeries";
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            {
                using var newTimeSeries = instance.CreateStandardTimeSeries(name, true);
                number = newTimeSeries.Number;
            }
            instance.RemoveStandardTimeSeries(number);
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void RemoveStandardTimeSeriesByName()
    {
        const string name = "NewTimeSeries";
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var newTimeSeries = instance.CreateStandardTimeSeries(name, true);
            instance.RemoveStandardTimeSeries(name);
        }
        finally
        {
            instance.Dispose();
        }
    }

}
