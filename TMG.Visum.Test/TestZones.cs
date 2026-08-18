namespace TMG.Visum.Test;

[TestClass]
public class TestZones
{
    [TestMethod]
    public void ZoneNumberCount()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            Assert.AreEqual(3, instance.GetZoneCount());
        }
        finally
        {
            instance.Dispose();
        }
    }

    [TestMethod]
    public void GetZoneNumbers()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            var zones = instance.GetZoneNumbers();
            Assert.AreEqual(3, zones.Length);
            for (int i = 0; i < zones.Length; i++)
            {
                Assert.AreEqual(i + 1, zones[i]);
            }
        }
        finally
        {
            instance.Dispose();
        }
    }

}
