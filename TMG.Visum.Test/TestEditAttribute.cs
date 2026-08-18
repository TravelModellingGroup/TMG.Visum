namespace TMG.Visum.Test;

[TestClass]
public class TestEditAttribute
{

    [TestMethod]
    public void EditAttribute()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            instance.ExecuteEditAttribute(
                new EditAttributeParameters()
                {
                    Formula = "123",
                    NetObjectType = "LINK",
                    ResultAttributeName = "LENGTH",
                    OnlyActive = false,
                });
            // TODO: Figure out how to automatically detect an error.
            // instance.SaveVersionFile("out.ver");
        }
        finally
        {
            instance.Dispose();
        }
    }

}
