using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    /// Thrown if the Visum instance has already been disposed.
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
    /// Thrown if the Visum instance has already been disposed.
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
            if (!TryGetMatrixInner(number, out IMatrix? matrix))
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
    public bool TryGetMatrix(int number, [NotNullWhen(true)] out VisumMatrix? matrix)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            if (TryGetMatrixInner(number, out var localMatrix))
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
    /// Tries to get a matrix with the given name.
    /// </summary>
    /// <param name="name">The name of the matrix to look for.</param>
    /// <param name="matrix">The matrix, null if no matrix was found.</param>
    /// <returns>True if a matrix with the name was found, false otherwise.</returns>
    public bool TryGetMatrixByName(string name, [NotNullWhen(true)] out VisumMatrix? matrix)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            if (!TryGetMatrixInner(name, out var localMatrix))
            {
                matrix = null;
                return false;
            }
            matrix = new VisumMatrix(localMatrix, ObjectTypeRefT.OBJECTTYPEREF_ZONE, _visum);
            return true;
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
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
    /// Use this call locally to avoid re-entering the locks.
    /// </summary>
    /// <param name="name">The matrix name to lookup</param>
    /// <param name="matrix">The matrix if it exists.</param>
    /// <returns>True if we found the matrix, false otherwise.</returns>
    private bool TryGetMatrixInner(string name, [NotNullWhen(true)] out IMatrix? matrix)
    {
        foreach (IMatrix m in _visum!.Net.Matrices)
        {
            if (m.GetName() == name)
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
    /// Thrown if the Visum instance has already been disposed or
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
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Get a matrix given the name.  If there are
    /// multiple matrices with the name, it will return the
    /// first one.
    /// </summary>
    /// <param name="name">The name of the matrix to look for.</param>
    /// <returns>The matrix with the given name.</returns>
    /// <exception cref="VisumException">Throws an exception if no matrix was found.</exception>
    public VisumMatrix GetMatrixByName(string name)
    {
        _lock.EnterReadLock();
        try
        {
            return GetMatrixByNameInner(name);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// INTERNAL ONLY:
    /// Get a matrix given the name.  If there are
    /// multiple matrices with the name, it will return the
    /// first one.
    /// 
    /// Must be called only if the write lock is held.
    /// </summary>
    /// <param name="name">The name of the matrix to look for.</param>
    /// <returns>The matrix with the given name.</returns>
    /// <exception cref="VisumException">Throws an exception if no matrix was found.</exception>
    internal VisumMatrix GetMatrixByNameInner(string name)
    {
        ObjectDisposedException.ThrowIf(_visum is null, this);
        if (!TryGetMatrixInner(name, out var matrix))
        {
            throw new VisumException($"Unable to find a matrix with the name {name}!");
        }
        return new VisumMatrix(matrix, ObjectTypeRefT.OBJECTTYPEREF_ZONE, _visum);
    }

    /// <summary>
    /// Delete the given matrix number.
    /// </summary>
    /// <param name="number">The matrix number to delete.</param>
    /// <exception cref="VisumException">
    /// Thrown if the Visum instance has already been disposed.
    /// </exception>
    public bool DeleteMatrix(int number)
    {
        _lock.EnterWriteLock();
        try
        {
            return DeleteMatrixInner(number);
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
    /// Delete the given matrix number.
    /// ONLY CALL FROM INTERNAL WHEN HOLDING A WRITE LOCK
    /// </summary>
    /// <param name="number">The matrix number to delete.</param>
    /// <exception cref="VisumException">
    /// Thrown if the Visum instance has already been disposed.
    /// </exception>
    internal bool DeleteMatrixInner(int number)
    {
        if (TryGetMatrixInner(number, out var matrix))
        {
            _visum?.Net.RemoveMatrix(matrix);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Delete the matrices with the given code
    /// </summary>
    /// <param name="number">The code to delete.</param>
    /// <exception cref="VisumException">
    /// Thrown if the Visum instance has already been disposed.
    /// </exception>
    public bool DeleteMatrixByCode(string code)
    {
        _lock.EnterWriteLock();
        try
        {
            return DeleteMatrixByCodeInner(code);
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
    /// Remove all instances of matrices with the given code.
    /// </summary>
    /// <param name="code">The code to remove.</param>
    /// <returns>True if at least one matrix was removed.</returns>
    internal bool DeleteMatrixByCodeInner(string code)
    {
        if(_visum is null)
        {
            return false;
        }
        var toRemove = new List<IMatrix>();
        foreach(IMatrix matrix in _visum.Net.Matrices)
        {
            if(matrix.GetCode().Equals(code, StringComparison.OrdinalIgnoreCase))
            {
                toRemove.Add(matrix);
            }
        }
        foreach( var matrix in toRemove)
        {
            _visum.Net.RemoveMatrix(matrix);
        }
        return toRemove.Count > 0;
    }

    /// <summary>
    /// Delete the matrices with the given name.
    /// </summary>
    /// <param name="number">The name to delete.</param>
    /// <exception cref="VisumException">
    /// Thrown if the Visum instance has already been disposed.
    /// </exception>
    public bool DeleteMatrixByName(string name)
    {
        _lock.EnterWriteLock();
        try
        {
            return DeleteMatrixByNameInner(name);
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
    /// Remove all instances of matrices with the given name.
    /// </summary>
    /// <param name="name">The name to remove.</param>
    /// <returns>True if at least one matrix was removed.</returns>
    internal bool DeleteMatrixByNameInner(string name)
    {
        if (_visum is null)
        {
            return false;
        }
        var toRemove = new List<IMatrix>();
        foreach (IMatrix matrix in _visum.Net.Matrices)
        {
            if (matrix.GetName().Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                toRemove.Add(matrix);
            }
        }
        foreach (var matrix in toRemove)
        {
            _visum.Net.RemoveMatrix(matrix);
        }
        return toRemove.Count > 0;
    }
}
