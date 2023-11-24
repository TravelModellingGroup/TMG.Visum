namespace TMG.Visum.Test;

[TestClass]
public class TestStandardTimeSeries
{

    [TestMethod]
    public void CreateStandardTimeSeries()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var newTimeSeries = instance.CreateStandardTimeSeries("NewTimeSeries", true);
    }

    [TestMethod]
    public void GetStandardTimeSeriesByNumber()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var timeSeries = instance.GetStandardTimeSeries(1);
    }

    [TestMethod]
    public void GetStandardTimeSeriesByName()
    {
        const string name = "NewTimeSeries";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var newTimeSeries = instance.CreateStandardTimeSeries(name, true);
        using var foundTimeSeries = instance.GetStandardTimeSeries(name);
        Assert.AreEqual(newTimeSeries.Number, foundTimeSeries.Number);
    }

    [TestMethod]
    public void RemoveStandardTimeSeriesByReference()
    {
        const string name = "NewTimeSeries";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var newTimeSeries = instance.CreateStandardTimeSeries(name, true);
        instance.RemoveStandardTimeSeries(newTimeSeries);
    }

    [TestMethod]
    public void RemoveStandardTimeSeriesByNumber()
    {
        int number;
        const string name = "NewTimeSeries";
        using var instance = new VisumInstance("TestNetwork.ver");
        {
            using var newTimeSeries = instance.CreateStandardTimeSeries(name, true);
            number = newTimeSeries.Number;
        }
        instance.RemoveStandardTimeSeries(number);
    }

    [TestMethod]
    public void RemoveStandardTimeSeriesByName()
    {
        const string name = "NewTimeSeries";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var newTimeSeries = instance.CreateStandardTimeSeries(name, true);
        instance.RemoveStandardTimeSeries(name);
    }

}
