using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Test;

[TestClass]
public class TestRoadAssignment
{
    [TestMethod]
    public void BasicRoadAssignmentEquilibrium()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var carSegment = instance.GetDemandSegment("C");
            using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
            carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            carSegment.DemandMatrix = carDemand;
            instance.ExecuteRoadAssignment(carSegment, new EquilibriumAssignment(new StabilityCriteria()));
        }
        finally { instance.Dispose(); }
    }

    [TestMethod]
    public void MultiClassRoadAssignmentEquilibrium()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var carSegment = instance.GetDemandSegment("C");
            using var bikeSegment = instance.GetDemandSegment("Bicycle");
            using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
            using var bikeDemand = instance.CreateDemandMatrix(2, "Bike demand");
            carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            bikeDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 1.0f).ToArray());
            carSegment.DemandMatrix = carDemand;
            bikeSegment.DemandMatrix = bikeDemand;
            instance.ExecuteRoadAssignment(new[] { carSegment, bikeSegment }, new EquilibriumAssignment(new StabilityCriteria()));
        }
        finally { instance.Dispose(); }
    }

    [TestMethod]
    public void BasicRoadAssignmentBFW()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var carSegment = instance.GetDemandSegment("C");
            using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
            carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            carSegment.DemandMatrix = carDemand;
            instance.ExecuteRoadAssignment(carSegment, new BWFAssignment(new StabilityCriteria()));
        }
        finally { instance.Dispose(); }
    }

    [TestMethod]
    public void MultipleRoadAssignments()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var carSegment = instance.GetDemandSegment("C");
            using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
            carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            carSegment.DemandMatrix = carDemand;
            instance.ExecuteRoadAssignment(carSegment, new BWFAssignment(new StabilityCriteria()));
            instance.ExecuteRoadAssignment(carSegment, new BWFAssignment(new StabilityCriteria()));
        }
        finally { instance.Dispose(); }
    }

    [TestMethod]
    public void MultiClassRoadAssignmentBFW()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var carSegment = instance.GetDemandSegment("C");
            using var bikeSegment = instance.GetDemandSegment("Bicycle");
            using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
            using var bikeDemand = instance.CreateDemandMatrix(2, "Bike demand");
            carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            bikeDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 1.0f).ToArray());
            carSegment.DemandMatrix = carDemand;
            bikeSegment.DemandMatrix = bikeDemand;
            instance.ExecuteRoadAssignment([carSegment, bikeSegment], new BWFAssignment(new StabilityCriteria()));
        }
        finally { instance.Dispose(); }
    }

    [TestMethod]
    public void BasicRoadAssignmentLUCE()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var carSegment = instance.GetDemandSegment("C");
            using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
            carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            carSegment.DemandMatrix = carDemand;
            instance.ExecuteRoadAssignment(carSegment, new LUCEAssignment(new StabilityCriteria()));
        }
        finally { instance.Dispose(); }
    }

    [TestMethod]
    public void MultiClassRoadAssignmentLUCE()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var carSegment = instance.GetDemandSegment("C");
            using var bikeSegment = instance.GetDemandSegment("Bicycle");
            using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
            using var bikeDemand = instance.CreateDemandMatrix(2, "Bike demand");
            carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            bikeDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 1.0f).ToArray());
            carSegment.DemandMatrix = carDemand;
            bikeSegment.DemandMatrix = bikeDemand;
            instance.ExecuteRoadAssignment([carSegment, bikeSegment], new LUCEAssignment(new StabilityCriteria()));
        }
        finally { instance.Dispose(); }
    }

    [TestMethod]
    public void BasicRoadAssignmentBicycle()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var bikeSegment = instance.GetDemandSegment("Bicycle");
            using var bikeDemand = instance.CreateDemandMatrix(2, "Bike demand");
            bikeDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            bikeSegment.DemandMatrix = bikeDemand;
            instance.ExecuteRoadAssignment(bikeSegment, new BicycleAssignment([bikeSegment]));
        }
        finally { instance.Dispose(); }
    }

    [TestMethod]
    public void MultiClassRoadAssignmentBicycle()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var carSegment = instance.GetDemandSegment("C");
            using var bikeSegment = instance.GetDemandSegment("Bicycle");
            using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
            using var bikeDemand = instance.CreateDemandMatrix(2, "Bike demand");
            carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            bikeDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 1.0f).ToArray());
            carSegment.DemandMatrix = carDemand;
            bikeSegment.DemandMatrix = bikeDemand;
            instance.ExecuteRoadAssignment([carSegment, bikeSegment], new BicycleAssignment([carSegment, bikeSegment]));
        }
        finally { instance.Dispose(); }
    }
}
