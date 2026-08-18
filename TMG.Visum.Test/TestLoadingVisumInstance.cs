namespace TMG.Visum.Test
{
    [TestClass]
    public class TestLoadingVisumInstance
    {
        [TestMethod]
        public void LoadVisumWithoutVersionFile()
        {
            var visum = new VisumInstance();
            try
            {
            }
            finally
            {
                visum.Dispose();
            }
        }

        [TestMethod]
        public void LoadVisumWithVersionFile()
        {
            var visum = new VisumInstance("BlankTestFile.ver");
            try
            {
            }
            finally
            {
                visum.Dispose();
            }
        }
    }
}
