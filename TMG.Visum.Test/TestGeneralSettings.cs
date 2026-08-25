using System.IO.Enumeration;

namespace TMG.Visum.Test;

[TestClass]
public class TestGeneralSettings
{
    [TestMethod]
    public void TestLoadGeneralSettings()
    {
        const string fileName = "GeneralSettings.xml";
        if (!File.Exists(fileName))
        {
            Assert.Fail($"The test file {fileName} does not exist!");
        }
        using var visum = new VisumInstance();
        visum.LoadGeneralSettings(fileName);
        // Add assertions to verify that the settings were loaded correctly
    }

    [TestMethod]
    public void TestSaveGeneralSettings()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            using var visum = new VisumInstance();
            visum.SaveGeneralSettings("GeneralSettings.xml");
            // Add assertions to verify that the settings were saved correctly
            Assert.IsTrue(File.Exists(tempFileName));
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
}
