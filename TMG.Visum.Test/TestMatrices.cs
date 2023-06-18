namespace TMG.Visum.Test;

[TestClass]
public class TestMatrices
{
    [TestMethod]
    public void CreateMatrix()
    {
        const string name = "TestMatrix";
        using var instance = new VisumInstance();
        using var matrix = instance.CreateDemandMatrix(1, name);
        Assert.IsNotNull(matrix);
        Assert.AreEqual(name, matrix.Name);
    }

    [TestMethod]
    public void TryGetNonExistingMatrix()
    {
        using var instance = new VisumInstance();
        Assert.IsFalse(instance.TryGetMatrix(1, out var matrix));
        Assert.IsNull(matrix);
    }

    [TestMethod]
    public void GetNonExistingMatrix()
    {
        using var instance = new VisumInstance();
        Assert.ThrowsException<VisumException>(() => _ = instance.GetMatrix(1));
    }

    [TestMethod]
    public void DeleteMatrix()
    {
        const string name = "TestMatrix";
        using var instance = new VisumInstance();
        using var matrix = instance.CreateDemandMatrix(1, name);
        Assert.IsNotNull(matrix);
        Assert.AreEqual(name, matrix.Name);
        Assert.IsTrue(instance.DeleteMatrix(1));
        Assert.IsFalse(instance.DeleteMatrix(1));
    }

    [TestMethod]
    public void CreatingMatrixWithZones()
    {
        const string name = "TestMatrix";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var matrix = instance.CreateDemandMatrix(1, name);
        Assert.IsNotNull(matrix);
        Assert.AreEqual(name, matrix.Name);
        Assert.AreEqual(0, matrix.Sum());
        Assert.AreEqual(3, matrix.Rows);
        Assert.AreEqual(3, matrix.Columns);
    }

    [TestMethod]
    public void GettingValues2DDouble()
    {
        const string name = "TestMatrix";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var matrix = instance.CreateDemandMatrix(1, name);
        Assert.IsNotNull(matrix);
        Assert.AreEqual(name, matrix.Name);
        Assert.AreEqual(0, matrix.Sum());
        Assert.AreEqual(3, matrix.Rows);
        Assert.AreEqual(3, matrix.Columns);

        var valuesBack = matrix.GetValuesAsDoubleMatrix();
        Assert.IsNotNull(valuesBack);
        Assert.AreEqual(3, valuesBack.Length, "The row size is not expected!");
        Assert.AreEqual(3, valuesBack[0].Length, "The row size is not expected!");
    }

    [TestMethod]
    public void SettingAndGettingValues1DFloat()
    {
        const string name = "TestMatrix";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var matrix = instance.CreateDemandMatrix(1, name);
        Assert.IsNotNull(matrix);
        Assert.AreEqual(name, matrix.Name);
        Assert.AreEqual(0, matrix.Sum());
        Assert.AreEqual(3, matrix.Rows);
        Assert.AreEqual(3, matrix.Columns);
        var data = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        matrix.SetValues(data);
        var valuesBack = matrix.GetValuesAsFloatArray();
        Assert.IsNotNull(valuesBack, "We got back a null when reading the array back.");
        Assert.AreEqual(9, data.Length, "The number of rows are wrong when reading it back.");

        CompareMatrix(data, valuesBack);
    }

    [TestMethod]
    public void SettingAndGettingValues2DFloat()
    {
        const string name = "TestMatrix";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var matrix = instance.CreateDemandMatrix(1, name);
        Assert.IsNotNull(matrix);
        Assert.AreEqual(name, matrix.Name);
        Assert.AreEqual(0, matrix.Sum());
        Assert.AreEqual(3, matrix.Rows);
        Assert.AreEqual(3, matrix.Columns);
        var data = new float[][]
        {
            new float[] {1,2,3},
            new float[] {4,5,6},
            new float[] {7,8,9},
        };
        matrix.SetValues(data);
        var valuesBack = matrix.GetValuesAsFloatMatrix();
        Assert.IsNotNull(valuesBack, "We got back a null when reading the array back.");
        Assert.AreEqual(3, data.Length, "The number of rows are wrong when reading it back.");
        Assert.AreEqual(3, data[0].Length, "The number of columns are wrong when reading it back.");

        CompareMatrix(data, valuesBack);
    }

    [TestMethod]
    public void SettingAndGettingValues1DDouble()
    {
        const string name = "TestMatrix";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var matrix = instance.CreateDemandMatrix(1, name);
        Assert.IsNotNull(matrix);
        Assert.AreEqual(name, matrix.Name);
        Assert.AreEqual(0, matrix.Sum());
        Assert.AreEqual(3, matrix.Rows);
        Assert.AreEqual(3, matrix.Columns);
        var data = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        matrix.SetValues(data);
        var valuesBack = matrix.GetValuesAsDoubleArray();
        Assert.IsNotNull(valuesBack, "We got back a null when reading the array back.");
        Assert.AreEqual(9, data.Length, "The number of rows are wrong when reading it back.");

        CompareMatrix(data, valuesBack);
    }

    [TestMethod]
    public void SettingAndGettingValues2DDouble()
    {
        const string name = "TestMatrix";
        using var instance = new VisumInstance("TestNetwork.ver");
        using var matrix = instance.CreateDemandMatrix(1, name);
        Assert.IsNotNull(matrix);
        Assert.AreEqual(name, matrix.Name);
        Assert.AreEqual(0, matrix.Sum());
        Assert.AreEqual(3, matrix.Rows);
        Assert.AreEqual(3, matrix.Columns);
        var data = new double[][]
        {
            new double[] {1,2,3},
            new double[] {4,5,6},
            new double[] {7,8,9},
        };
        matrix.SetValues(data);
        var valuesBack = matrix.GetValuesAsDoubleMatrix();
        Assert.IsNotNull(valuesBack, "We got back a null when reading the array back.");
        Assert.AreEqual(3, data.Length, "The number of rows are wrong when reading it back.");
        Assert.AreEqual(3, data[0].Length, "The number of columns are wrong when reading it back.");

        CompareMatrix(data, valuesBack);
    }

    private static void CompareMatrix(float[] data, float[] valuesBack)
    {
        for (int i = 0; i < data.Length; i++)
        {
            Assert.AreEqual(data[i], valuesBack[i], 0.0001f);
        }
    }

    private static void CompareMatrix(float[][] data, float[][] valuesBack)
    {
        for (int i = 0; i < data.Length; i++)
        {
            for(int j = 0; j < data[i].Length; j++)
            {
                Assert.AreEqual(data[i][j], valuesBack[i][j], 0.0001f);
            }
        }
    }

    private static void CompareMatrix(double[] data, double[] valuesBack)
    {
        for (int i = 0; i < data.Length; i++)
        {
            Assert.AreEqual(data[i], valuesBack[i], 0.0001);
        }
    }

    private static void CompareMatrix(double[][] data, double[][] valuesBack)
    {
        for (int i = 0; i < data.Length; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                Assert.AreEqual(data[i][j], valuesBack[i][j], 0.0001);
            }
        }
    }
}
