namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    public void ExportShapeFile(string filePath, ShapeFileType type)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            var fullPath = Path.GetFullPath(filePath);
            var parameters = _visum.IO.CreateExportShapeFilePara();
            parameters.ObjectType = type.ToInternalType();
            
            _visum.IO.ExportShapefile(fullPath, parameters);
            COM.ReleaseCOMObject(ref parameters);
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
}

public enum ShapeFileType
{
    /// <summary>
    /// 
    /// </summary>
    Node = 1,

    /// <summary>
    /// 
    /// </summary>
    Link = 2,

    /// <summary>
    /// 
    /// </summary>
    Zone = 3,

    /// <summary>
    /// 
    /// </summary>
    PointPOI = 4,

    /// <summary>
    ///     
    /// </summary>
    LineRoutes = 5,

    TimeProfile = 6,
}

internal static class ShapeFileTypeExtensions
{
    internal static ShapeFileObjType ToInternalType(this ShapeFileType type)
    {
        return type switch
        {
            ShapeFileType.Node => ShapeFileObjType.shapefileTypeNodes,
            ShapeFileType.Link => ShapeFileObjType.shapefileTypeLinks,
            ShapeFileType.Zone => ShapeFileObjType.shapefileTypeZones,
            ShapeFileType.PointPOI => ShapeFileObjType.shapefileTypePOIs,
            ShapeFileType.LineRoutes => ShapeFileObjType.shapefileTypeLineRoutes,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
