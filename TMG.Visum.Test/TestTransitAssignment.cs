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
            using var transitDemand = instance.CreateDemandMatrix(1, "X demand");
            // Assign 3 demand for all OD.
            transitDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            transitSegment.DemandMatrix = transitDemand;
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
            //DEBUG: instance.SaveVersionFile("Temp.ver");
        }
    }

    [TestMethod]
    public void MultipleHeadwayAssignments()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var transitSegment = instance.GetDemandSegment("X");
            using var transitDemand = instance.CreateDemandMatrix(1, "X demand");
            // Assign 3 demand for all OD.
            transitDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            transitSegment.DemandMatrix = transitDemand;
            var matrices = instance.ExecuteTransitAssignment(transitSegment,
                new PutLoSTypes[]
                {
                    PutLoSTypes.PerceivedJourneyTime,
                    PutLoSTypes.JourneyTime,
                },
                new HeadwayImpedanceParameters());
            DisposeMatrices(matrices);
            // Execute a second transit assignment
            matrices = instance.ExecuteTransitAssignment(transitSegment,
                new PutLoSTypes[]
                {
                    PutLoSTypes.PerceivedJourneyTime,
                    PutLoSTypes.JourneyTime,
                },
                new HeadwayImpedanceParameters());
        }
        finally
        {
            //DEBUG: instance.SaveVersionFile("Temp.ver");
        }
    }

    [TestMethod]
    public void TestTransitAssignmentWithMultipleDays()
    {
        //using var instance = new VisumInstance(@"Z:\Projects\2023\Halifax\V4Input\BaseNetwork.ver");
        using var instance = new VisumInstance(@"TestNetwork-Calendar.ver");
        try
        {
            using var transitSegment = instance.GetDemandSegment("X");
            using var transitDemand = instance.CreateDemandMatrix(1, "X demand");
            var numberOfZones = instance.GetZoneCount();
            // Assign 3 demand for all OD.
            transitDemand.SetValues(Enumerable.Range(0, numberOfZones * numberOfZones).Select(_ => 0.01f).ToArray());
            transitSegment.DemandMatrix = transitDemand;
            var matrices = instance.ExecuteTransitAssignment(transitSegment,
                new PutLoSTypes[]
                {
                    PutLoSTypes.PerceivedJourneyTime,
                    PutLoSTypes.JourneyTime,
                },
                new HeadwayImpedanceParameters()
                {
                    AssignmentStartDayIndex = 122,
                    AssignmentEndDayIndex = 122,
                    AssignmentStartTime = TimeOnly.Parse("17:00:00"),
                    AssignmentEndTime = TimeOnly.Parse("18:00:00")
                });
            DisposeMatrices(matrices);
        }
        finally
        {
            //DEBUG: instance.SaveVersionFile("Temp.ver");
        }
    }


    [TestMethod]
    public void TestGetLineBoardings()
    {
        using var instance = new VisumInstance("TestNetwork.ver");
        using var transitSegment = instance.GetDemandSegment("X");
        using var transitDemand = instance.CreateDemandMatrix(1, "X demand");
        // Assign 3 demand for all OD.
        transitDemand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
        transitSegment.DemandMatrix = transitDemand;
        var matrices = instance.ExecuteTransitAssignment(transitSegment,
            new PutLoSTypes[]
            {
                    PutLoSTypes.PerceivedJourneyTime,
                    PutLoSTypes.JourneyTime,
            },
            new HeadwayImpedanceParameters());
        var boardings = instance.GetBoardings();
        Assert.IsNotNull(boardings);
        Assert.AreEqual(1, boardings.Count);
        Assert.IsTrue(boardings.Sum(line => line.boardings) > 0);
        DisposeMatrices(matrices);
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
