namespace TMG.Visum.Test;

[TestClass]
public class TestUserAttributes
{

    [TestMethod]
    public void TestCreateUserAttribute()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            Assert.IsTrue(instance.CheckAttributeExists("USERNODEATTRIBUTE", NetworkObjectType.Node));
        }
        finally
        {
            instance.Dispose();
        }
    }

}
