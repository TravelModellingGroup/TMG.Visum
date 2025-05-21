using TMG.Visum.RoadAssignment;

namespace TMG.Visum.Test;

[TestClass]
public class TestRoadLoS
{
    [TestMethod]
    public void CongestedTravelTimes()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var carSegment = instance.GetDemandSegment("C");
        using var carDemand = instance.CreateDemandMatrix(1, "Car demand");
        // Assign 3 demand for all OD.
        carDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
        carSegment.DemandMatrix = carDemand;
        instance.ExecuteRoadAssignment(carSegment,
            new LUCEAssignment(new StabilityCriteria()));

        using var travelTimeMatrix = instance.CalculateRoadLoS(carSegment, PrTLosTypes.TCur, PrTLoSSearchCriterion.Impedance);
        Assert.AreEqual("tCur C", travelTimeMatrix.Name);
        Assert.AreEqual(3, travelTimeMatrix.Rows);
        Assert.AreEqual(3, travelTimeMatrix.Columns);
        using var uncongestedTravelTimeMatrix = instance.CalculateRoadLoS(carSegment, PrTLosTypes.T0, PrTLoSSearchCriterion.Impedance);
        Assert.AreEqual("t0 C", uncongestedTravelTimeMatrix.Name);
        Assert.AreEqual(3, uncongestedTravelTimeMatrix.Rows);
        Assert.AreEqual(3, uncongestedTravelTimeMatrix.Columns);
        var uncongested = uncongestedTravelTimeMatrix.GetValuesAsFloatArray();
        var congested = travelTimeMatrix.GetValuesAsFloatArray();
        for (int i = 0; i < uncongested.Length; i++)
        {
            Assert.IsTrue(uncongested[i] <= congested[i]);
        }
        Assert.IsTrue(uncongested.Sum() < congested.Sum(),
            $"The uncontested road travel times is not less than the sum of congested travel times! {uncongested.Sum()} to {congested.Sum()}");
    }

}
