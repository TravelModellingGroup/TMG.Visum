namespace TMG.Visum.Test;

[TestClass]
public class TestUserAttributes
{

    [TestMethod]
    public void TestCreateUserAttribute()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        Assert.IsTrue(instance.CheckAttributeExists("USERNODEATTRIBUTE", NetworkObjectType.Node));
    }

}
