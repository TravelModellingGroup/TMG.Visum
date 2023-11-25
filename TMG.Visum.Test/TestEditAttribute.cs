namespace TMG.Visum.Test;

[TestClass]
public class TestEditAttribute
{

    [TestMethod]
    public void EditAttribute()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        instance.RunEditAttribute("123", "LINK", "LENGTH", false);
        // TODO: Figure out how to automatically detect an error.
        // instance.SaveVersionFile("out.ver");
    }

}
