using TMG;

namespace TMG.Visum.Import;

[Module(
    Description = "Store the given matrix into a Visum Instance.",
    Name = "Import Matrix To Visum",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Import/ImportMatrixToVisum.html"
    )]
public sealed class ImportMatrixToVisum : BaseAction<VisumInstance>
{
    [Parameter(Name = "Matrix Number", DefaultValue = "0", Description = "The matrix number to store into.", Index = 0)]
    public IFunction<int> MatrixNumber = null!;

    [Parameter(Name = "Matrix Name", DefaultValue = "", Description = "The name to associate with the matrix.", Index = 1)]
    public IFunction<string> MatrixName = null!;

    [Parameter(Name = "Matrix", Description = "The matrix to store into Visum.", Index = 2)]
    public IFunction<Matrix> ToSave = null!;

    public override void Invoke(VisumInstance instance)
    {
        VisumMatrix? matrix = null;
        try
        {
            var matrixNumber = MatrixNumber.Invoke();
            var matrixName = MatrixName.Invoke();

            if (!instance.TryGetMatrix(matrixNumber, out matrix))
            {
                matrix = instance.CreateDemandMatrix(matrixNumber, matrixName);
            }
            else
            {
                matrix.Name = matrixName;
                matrix.SetAsDemandMatrix();
            }
            var data = ToSave.Invoke().Data.ToArray();
            matrix.SetValues(data);
        }
        catch (VisumException ex)
        {
            throw new XTMFRuntimeException(this, "Unable to import matrix into Visum", ex);
        }
        finally
        {
            matrix?.Dispose();
        }
    }

    public override bool RuntimeValidation(ref string? error)
    {
        if (string.IsNullOrWhiteSpace(MatrixName.Invoke()))
        {
            error = "The matrix name can not be blank!";
            return false;
        }
        return true;
    }
}
