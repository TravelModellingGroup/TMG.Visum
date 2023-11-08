using System.Diagnostics.CodeAnalysis;
using VISUMLIB;

namespace TMG.Visum;

/// <summary>
/// Provides an interface for working with a VISUM matrix
/// </summary>
public sealed class VisumMatrix : IDisposable
{
    /// <summary>
    /// Our local copy of the Visum matrix.
    /// </summary>
    private IMatrix _matrix;

    /// <summary>
    /// Store what type of data this matrix represents.
    /// </summary>
    private readonly ObjectTypeRefT _basedOn;

    /// <summary>
    /// The instance of visum that this matrix belongs to.
    /// </summary>
    private readonly IVisum _instance;

    /// <summary>
    /// Provides a wrapper around the matrix that
    /// allows us to deal with COM properly.
    /// </summary>
    /// <param name="matrix">The matrix to wrap.</param>
    /// <param name="basedOn">The type of attribute that the matrix was based on.</param>
    /// <param name="visum">The visum instance to work with.</param>
    internal VisumMatrix(IMatrix matrix, ObjectTypeRefT basedOn, IVisum visum)
    {
        _matrix = matrix;
        _basedOn = basedOn;
        _instance = visum;
    }

    /// <summary>
    /// Internal only, get access to the underlying matrix.
    /// </summary>
    internal IMatrix Matrix => _matrix;

    /// <summary>
    /// Get the dimensions of from the given reference type.
    /// </summary>
    /// <param name="basedOn">The type of matrix to create.</param>
    /// <param name="visum">Our visum instance needed to get the sizes from.</param>
    /// <returns>The number of rows and columns for the matrix.</returns>
    /// <exception cref="InvalidOperationException">If an unknown type of matrix is requested.</exception>
    private static (int rows, int columns) GetDimensions(ObjectTypeRefT basedOn, IVisum visum)
    {
        int count = basedOn switch
        {
            ObjectTypeRefT.OBJECTTYPEREF_ZONE => visum.Net.Zones.Count,
            ObjectTypeRefT.OBJECTTYPEREF_MAINZONE => visum.Net.MainZones.Count,
            ObjectTypeRefT.OBJECTTYPEREF_STOPAREA => visum.Net.StopAreas.Count,
            _ => ThrowInvalidType<int>(basedOn)
        };
        return (count, count);
    }

    public int[] GetSparseIndexes()
    {
        return _basedOn switch
        {
            ObjectTypeRefT.OBJECTTYPEREF_ZONE => _instance.Net.Zones.GetZoneNumbers(),
            ObjectTypeRefT.OBJECTTYPEREF_MAINZONE => _instance.Net.MainZones.GetZoneNumbers(),
            ObjectTypeRefT.OBJECTTYPEREF_STOPAREA => _instance.Net.StopAreas.GetZoneNumbers(),
            _ => ThrowInvalidType<int[]>(_basedOn)
        };
    }

    [DoesNotReturn]
    private static T ThrowInvalidType<T>(ObjectTypeRefT basedOn)
    {
        throw new InvalidOperationException($"The matrix type {Enum.GetName(typeof(ObjectTypeRefT), basedOn)} is not supported!");
    }

    /// <summary>
    /// The associated name for the matrix
    /// </summary>
    public string Name
    {
        get => _matrix.GetName();
        set => _matrix.SetName(value);
    }

    /// <summary>
    /// The associated code for the matrix (short name)
    /// </summary>
    public string Code
    {
        get => _matrix.GetCode();
        set => _matrix.SetCode(value);
    }

    /// <summary>
    /// The number of rows in the matrix
    /// </summary>
    public int Rows => GetDimensions(_basedOn, _instance).rows;

    /// <summary>
    /// The number of columns in the matrix
    /// </summary>
    public int Columns => GetDimensions(_basedOn, _instance).columns;

    /// <summary>
    /// Gets the total sum of the matrix
    /// </summary>
    public double Sum() => _matrix.GetODSum();

    #region GetValues

    /// <summary>
    /// Get the values of the matrix stored in a 2D array.
    /// </summary>
    /// <returns>A 2D array of floats with the values of the matrix.</returns>
    public float[][] GetValuesAsFloatMatrix()
    {
        return ConvertToArrayOfArrays((float[,])_matrix.GetValuesFloat());
    }

    /// <summary>
    /// Get the values of the matrix stored in a 2D array.
    /// </summary>
    /// <returns>A 2D array of doubles with the values of the matrix.</returns>
    public double[][] GetValuesAsDoubleMatrix()
    {
        return ConvertToArrayOfArrays((double[,])_matrix.GetValuesDouble());
    }

    /// <summary>
    /// Get the values of the matrix stored in a flat array.
    /// </summary>
    /// <returns>A 1D array of floats with the values of the matrix.</returns>
    public float[] GetValuesAsFloatArray()
    {
        return ConvertToArray((float[,])_matrix.GetValuesFloat());
    }

    /// <summary>
    /// Get the values of the matrix stored in a flat array.
    /// </summary>
    /// <returns>A 1D array of doubles with the values of the matrix.</returns>
    public double[] GetValuesAsDoubleArray()
    {
        return ConvertToArray((double[,])_matrix.GetValuesDouble());
    }

    private T[,] ConvertTo2DMatrix<T>(T[] flat)
    {
        var rows = Rows;
        var columns = Columns;
        var ret = new T[rows,columns];
        if(ret.Length != flat.Length)
        {
            throw new VisumException($"The size of the arrays are not the same expected {ret.Length} but received {flat.Length}!");
        }
        int pos = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                ret[i, j] = flat[pos++];
            }
        }
        return ret;
    }

    private T[,] ConvertTo2DMatrix<T>(T[][] flat)
    {
        var rows = Rows;
        var columns = Columns;
        var ret = new T[rows, columns];
        if (rows != flat.Length)
        {
            throw new VisumException($"The size of the arrays are not the same expected {Rows} but received {flat.Length}!");
        }
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                ret[i, j] = flat[i][j];
            }
        }
        return ret;
    }

    #endregion

    #region SetValues

    /// <summary>
    /// Store the given values to the matrix
    /// </summary>
    /// <param name="matrix">The data to store to the matrix</param>
    public void SetValues(float[][] matrix)
    {
        _matrix.SetValuesFloat(ConvertTo2DMatrix(matrix));
    }

    /// <summary>
    /// Store the given values to the matrix
    /// </summary>
    /// <param name="matrix">The data to store to the matrix</param>
    public void SetValues(float[] matrix)
    {
        _matrix.SetValuesFloat(ConvertTo2DMatrix(matrix));
    }

    /// <summary>
    /// Store the given values to the matrix
    /// </summary>
    /// <param name="matrix">The data to store to the matrix</param>
    public void SetValues(double[][] matrix)
    {
        _matrix.SetValuesDouble(ConvertTo2DMatrix(matrix));
    }

    /// <summary>
    /// Store the given values to the matrix
    /// </summary>
    /// <param name="matrix">The data to store to the matrix</param>
    public void SetValues(double[] matrix)
    {
        _matrix.SetValuesDouble(ConvertTo2DMatrix(matrix));
    }

    private static T[][] ConvertToArrayOfArrays<T>(T[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);
        var ret = new T[rows][];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = new T[cols];
            for (int j = 0; j < ret[i].Length; j++)
            {
                ret[i][j] = matrix[i,j];
            }
        }
        return ret;
    }

    private static T[] ConvertToArray<T>(T[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);
        var ret = new T[rows * cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                ret[i * cols + j] = matrix[i, j];
            }
        }
        return ret;
    }

    #endregion

    #region IDispose

    private bool disposedValue;

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                COM.ReleaseCOMObject(ref _matrix!, false);
            }
            disposedValue = true;
        }
    }

    ~VisumMatrix()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion

}
