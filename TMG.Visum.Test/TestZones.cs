namespace TMG.Visum.Test;

[TestClass]
public class TestZones
{
    [TestMethod]
    public void ZoneNumberCount()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        Assert.AreEqual(3, instance.GetZoneCount());
    }

    [TestMethod]
    public void GetZoneNumbers()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        var zones = instance.GetZoneNumbers();
        Assert.AreEqual(3, zones.Length);
        for (int i = 0; i < zones.Length; i++)
        {
            Assert.AreEqual(i + 1, zones[i]);
        }
    }

}
