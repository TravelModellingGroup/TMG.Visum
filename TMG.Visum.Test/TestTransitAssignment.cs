using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMG.Visum.TransitAssignment;

namespace TMG.Visum.Test;

[TestClass]
public class TestTransitAssignment
{
    [TestMethod]
    public void HeadwayTransitAssignment()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSegment = instance.GetDemandSegment("X");
            using var carDemand = instance.CreateDemandMatrix(1, "X demand");
            // Assign 3 demand for all OD.
            carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            transitSegment.DemandMatrix = carDemand;
            var matrices = instance.ExecuteTransitAssignment(transitSegment,
                new PutLoSTypes[]
                {
                    PutLoSTypes.PerceivedJourneyTime,
                    PutLoSTypes.JourneyTime,
                },
                new HeadwayImpedanceParameters());
            DisposeMatrices(matrices);
        }
        finally
        {
            instance.SaveVersionFile("Temp.ver");
        }
    }

    private static void DisposeMatrices(List<List<VisumMatrix>> matrices)
    {
        foreach (var matrix in matrices)
        {
            DisposeMatrices(matrix);
        }
    }

    private static void DisposeMatrices(List<VisumMatrix> matrices)
    {
        foreach (var matrix in matrices)
        {
            matrix.Dispose();
        }
    }
}
