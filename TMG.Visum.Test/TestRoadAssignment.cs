namespace TMG.Visum.Test;

[TestClass]
public class TestRoadAssignment
{
    [TestMethod]
    public void BasicRoadAssignment()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var carSegment = instance.GetDemandSegment("C");
        using var bikeSegment = instance.GetDemandSegment("Bicycle");
        using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
        using var bikeDemand = instance.CreateDemandMatrix(2, "Bike demand");
        // Assign 3 demand for all OD.
        carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
        bikeDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 1.0f).ToArray());
        carSegment.DemandMatrix = carDemand;
        bikeSegment.DemandMatrix = bikeDemand;
        instance.ExecuteRoadAssignment(new[] { carSegment, bikeSegment });
    }
}
