using System.Diagnostics.CodeAnalysis;

namespace TMG.Visum;

public partial class VisumInstance : IDisposable
{
    /// <summary>
    /// Create a new matrix with the given ID number
    /// </summary>
    /// <param name="number">The matrix number to create.</param>
    /// <param name="matrixName">The name to give the matrix.</param>
    /// <returns>The newly created matrix.</returns>
    /// <exception cref="VisumException">
    /// Thrown if the Visum instance has alreadby been disposed.
    /// </exception>
    public VisumMatrix CreateDemandMatrix(int number, string matrixName)
    {
        return CreateMatrix(number, matrixName, MatrixType.MATRIXTYPE_DEMAND);
    }

    /// <summary>
    /// Create a new matrix with the given ID number
    /// </summary>
    /// <param name="number">The matrix number to create.</param>
    /// <param name="matrixName">The name to give the matrix.</param>
    /// <returns>The newly created matrix.</returns>
    /// <exception cref="VisumException">
    /// Thrown if the Visum instance has alreadby been disposed.
    /// </exception>
    public VisumMatrix CreateSkimMatrix(int number, string matrixName)
    {
        return CreateMatrix(number, matrixName, MatrixType.MATRIXTYPE_SKIM);
    }

    private VisumMatrix CreateMatrix(int number, string matrixName, MatrixType type)
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            IMatrix? matrix;
            if(!TryGetMatrixInner(number, out matrix))
            {
                matrix = _visum.Net.AddMatrix(number,
                ObjectTypeRefT.OBJECTTYPEREF_ZONE,
                type);
            }
            matrix.Init();
            matrix.SetName(matrixName);
            return new VisumMatrix(matrix, ObjectTypeRefT.OBJECTTYPEREF_ZONE, _visum);
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Tries to get a matrix with the given number from the instance.
    /// </summary>
    /// <param name="number">The matrix number to try to get.</param>
    /// <param name="matrix">The matrix if it exists, null otherwise.</param>
    /// <returns>True if the matrix was found, false otherwise.</returns>
    public bool TryGetMatrix(int number, [NotNullWhen(true)]out VisumMatrix? matrix)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            if(TryGetMatrixInner(number, out var localMatrix))
            {
                matrix = new VisumMatrix(localMatrix, ObjectTypeRefT.OBJECTTYPEREF_ZONE, _visum);
                return true;
            }
            else
            {
                matrix = null;
                return false;
            }
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Use this call locally to avoid re-entering the locks.
    /// </summary>
    /// <param name="number">The matrix number to lookup</param>
    /// <param name="matrix">The matrix if it exists.</param>
    /// <returns>True if we found the matrix, false otherwise.</returns>
    private bool TryGetMatrixInner(int number, [NotNullWhen(true)] out IMatrix? matrix)
    {
        foreach (IMatrix m in _visum!.Net.Matrices)
        {
            if (m.GetNumber() == number)
            {
                matrix = m;
                return true;
            }
        }
        matrix = null;
        return false;
    }

    /// <summary>
    /// Get a reference to the matrix with the given number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="VisumException">
    /// Thrown if the Visum instance has alreadby been disposed or
    /// if the matrix does not exist.
    /// </exception>
    public VisumMatrix GetMatrix(int number)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            return new(_visum.Net.Matrices.ItemByKey[number], ObjectTypeRefT.OBJECTTYPEREF_ZONE, _visum);
        }
        catch(Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        { 
            _lock.ExitReadLock(); 
        }
    }

    /// <summary>
    /// Delete the given matrix number.
    /// </summary>
    /// <param name="number">The matrix number to delete.</param>
    /// <exception cref="VisumException">
    /// Thrown if the Visum instance has alreadby been disposed.
    /// </exception>
    public bool DeleteMatrix(int number)
    {
        _lock.EnterWriteLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            if (TryGetMatrixInner(number, out var matrix))
            {
                _visum.Net.RemoveMatrix(matrix);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}
