namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">The path to save the shape file to.</param>
    /// <param name="type">The type of parameters to export</param>
    /// <param name="extraAttributes">A list of additional extra attributes to export.</param>
    /// <param name="exclusivelyExtraAttributes">Should we only export the specified attributes?</param>
    public void ExportShapeFile(string filePath, ShapeFileType type, Span<string> extraAttributes, bool exclusivelyExtraAttributes)
    {
        _lock.EnterReadLock();
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            var fullPath = Path.GetFullPath(filePath);
            var parameters = _visum.IO.CreateExportShapeFilePara();
            parameters.ObjectType = type.ToInternalType();
            if(type == ShapeFileType.Link)
            {
                // Set the links to be directed by default
                parameters.AttValue["Directed"] = 1;
            }
            if (exclusivelyExtraAttributes)
            {
                // If we only want the extra attributes, we clear the default attributes
                parameters.ClearLayout();
            }
            foreach (var attr in extraAttributes)
            {
                parameters.AddColumn(attr);
            }
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
