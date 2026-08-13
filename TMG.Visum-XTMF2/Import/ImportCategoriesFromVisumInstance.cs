
namespace TMG.Visum.Import;

[Module(Name = "ImportCategoriesFromVisumInstance",
    Description = "Imports categories from a Visum instance",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Import/ImportCategoriesFromVisumInstance.html"
    )]
public sealed class ImportCategoriesFromVisumInstance : BaseFunction<VisumInstance, Categories>
{
    [SubModule(Required = false, Name = "X", Description = "The X coordinate of the zone", Index = 0, PassesExecution = true)]
    public ISetableValue<Vector>? X;

    [SubModule(Required = false, Name = "Y", Description = "The Y coordinate of the zone", Index = 1, PassesExecution = true)]
    public ISetableValue<Vector>? Y;

    override public Categories Invoke(VisumInstance context)
    {
        try
        {
            var data = context!.GetZoneInformation();
            string? error = null;
            if (!Categories.CreateCategories(data.zoneNumber.AsSpan(), out Categories? categories, ref error))
            {
                throw new Exception($"Failed to create categories from Visum instance: {error}");
            }

            // Check to see if the X and Y coordinates are requested to be set, and if so, set them
            if (X is not null)
            {
                var x = new Vector(categories);
                data.x.CopyTo(x.Data);
                X.Set(x);
            }

            if (Y is not null)
            {
                var y = new Vector(categories);
                data.y.CopyTo(y.Data);
                Y.Set(y);
            }

            return categories;
        }
        catch (VisumException ex)
        {
            throw new XTMFRuntimeException(this, $"Failed to import categories from Visum instance: {ex.Message}", ex);
        }
    }
}
