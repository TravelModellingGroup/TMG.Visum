using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Test;

[TestClass]
public class TestRoadAssignment
{

    [TestMethod]
    public void BasicRoadAssignmentEquilibrium()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var carSegment = instance.GetDemandSegment("C");
        using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
        // Assign 3 demand for all OD.
        carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
        carSegment.DemandMatrix = carDemand;
        instance.ExecuteRoadAssignment(carSegment,
            new RoadAssignment.StabilityCriteria(),
            new EquilibriumAssignment());
    }

    [TestMethod]
    public void MultiClassRoadAssignmentEquilibrium()
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
        instance.ExecuteRoadAssignment(new[] { carSegment, bikeSegment },
            new RoadAssignment.StabilityCriteria(),
            new EquilibriumAssignment());
    }

    [TestMethod]
    public void BasicRoadAssignmentBFW()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var carSegment = instance.GetDemandSegment("C");
        using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
        // Assign 3 demand for all OD.
        carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
        carSegment.DemandMatrix = carDemand;
        instance.ExecuteRoadAssignment(carSegment,
            new RoadAssignment.StabilityCriteria(),
            new BWFAssignment());
    }

    [TestMethod]
    public void MultiClassRoadAssignmentBFW()
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
        instance.ExecuteRoadAssignment(new[] { carSegment, bikeSegment },
            new RoadAssignment.StabilityCriteria(),
            new BWFAssignment());
    }

    [TestMethod]
    public void BasicRoadAssignmentLUCE()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var carSegment = instance.GetDemandSegment("C");
        using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
        // Assign 3 demand for all OD.
        carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
        carSegment.DemandMatrix = carDemand;
        instance.ExecuteRoadAssignment(carSegment,
            new RoadAssignment.StabilityCriteria(),
            new LUCEAssignment());
    }

    [TestMethod]
    public void MultiClassRoadAssignmentLUCE()
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
        instance.ExecuteRoadAssignment(new[] { carSegment, bikeSegment },
            new RoadAssignment.StabilityCriteria(),
            new LUCEAssignment());
    }
}
