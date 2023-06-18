namespace TMG.Visum.Test
{
    [TestClass]
    public class TestLoadingVisumInstance
    {
        [TestMethod]
        public void LoadVisumWithoutVersionFile()
        {
            using var visum = new VisumInstance();
        }

        [TestMethod]
        public void LoadVisumWithVersionFile()
        {
            using var visum = new VisumInstance("BlankTestFile.ver");
        }
    }
}
