using System.Reflection.Metadata;
using TMG.Visum.RoadAssignment;
using TMG.Visum.TransitAssignment;

namespace TMG.Visum.Test;

/// <summary>
/// Represents a test class for transit assignment.
/// </summary>
[TestClass]
public class TestTransitAssignment
{
    /// <summary>
    /// Tests the headway transit assignment.
    /// </summary>
    [TestMethod]
    public void HeadwayTransitAssignment()
    {
        var instance = new VisumInstance("TestNetwork.ver");
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
            instance.Dispose();
            instance = null;
            Utilities.ForceGC();
        }
    }

    /// <summary>
    /// Tests multiple headway assignments.
    /// </summary>
    [TestMethod]
    public void MultipleHeadwayAssignments()
    {
        var instance = new VisumInstance("TestNetwork.ver");
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
            matrices = null;
            // Execute a second transit assignment
            var matrices2 = instance.ExecuteTransitAssignment(transitSegment,
                new PutLoSTypes[]
                {
                    PutLoSTypes.PerceivedJourneyTime,
                    PutLoSTypes.JourneyTime,
                },
                new HeadwayImpedanceParameters());
            DisposeMatrices(matrices2);
            matrices2 = null;
        }
        finally
        {
            instance.Dispose();
            instance = null;
            Utilities.ForceGC();
        }
    }

    /// <summary>
    /// Tests transit assignment with multiple days.
    /// </summary>
    [TestMethod]
    public void TestTransitAssignmentWithMultipleDays()
    {
        //using var instance = new VisumInstance(@"Z:\Projects\2023\Halifax\V4Input\BaseNetwork.ver");
        var instance = new VisumInstance(@"TestNetwork-Calendar.ver");
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
            instance.Dispose();
            Utilities.ForceGC();
        }
    }

    /// <summary>
    /// Tests getting line boardings.
    /// </summary>
    [TestMethod]
    public void TestGetLineBoardings()
    {
        var instance = new VisumInstance("TestNetwork.ver");
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
            var boardings = instance.GetBoardings();
            Assert.IsNotNull(boardings);
            Assert.AreEqual(1, boardings.Count);
            Assert.IsTrue(boardings.Sum(line => line.boardings) > 0);
            DisposeMatrices(matrices);
        }
        finally
        {
            instance.Dispose();
            Utilities.ForceGC();
        }
    }

    /// <summary>
    /// Tests the STSU.
    /// </summary>
    [TestMethod]
    public void TestSTSU()
    {
        var instance = new VisumInstance("TestNetwork.ver");
        try
        {
            using var carSegment = instance.GetDemandSegment("C");
            using var transitSegment = instance.GetDemandSegment("X");
            using var demand = instance.CreateDemandMatrix(1, "X demand");
            // Assign 3 demand for all OD.
            demand.SetValues(Enumerable.Range(0, 9).Select(_ => 3.0f).ToArray());
            carSegment.DemandMatrix = demand;
            instance.ExecuteRoadAssignment(carSegment,
                new BWFAssignment(new StabilityCriteria()));
            transitSegment.DemandMatrix = demand;
            const float autoCorrelcation = 1.0f;
            // Speeds need to be in kmps not kmph
            const float defaultSpeed = 35.0f / 3600.0f;
            const float defaultStopDuration = 1.0f;
            var matrices = instance.ExecuteTransitAssignment(transitSegment,
                [
                    PutLoSTypes.PerceivedJourneyTime,
                    PutLoSTypes.JourneyTime,
                ],
                new HeadwayImpedanceParameters()
                {
                    STSUParameters =
                    [
                        new STSUParameters()
                        {
                            BoardingDuration = 0,
                            AlightingDuration = 0,
                            AutoCorrelation = autoCorrelcation,
                            AutoDemandSegment = "C",
                            DefaultEROWSpeed = defaultSpeed,
                            StopDuration = defaultStopDuration,
                            FilterFileName = "Bus.fil",
                            BusFacilityAttributeName = "ExclusiveBusFacility"
                        }
                    ]
                });

            string? error = null;
            var success = instance.TestSTSU(autoCorrelcation, defaultSpeed, defaultStopDuration, "C", ref error);
            Assert.IsTrue(success, error);
            DisposeMatrices(matrices);
        }
        finally
        {
            instance.Dispose();
        }
    }

    private static void DisposeMatrices(List<List<VisumMatrix>> matrices)
    {
        foreach (var matrix in matrices)
        {
            DisposeMatrices(matrix);
        }
        matrices.Clear();
    }

    /// <summary>
    /// Disposes the list of VisumMatrix objects.
    /// </summary>
    /// <param name="matrices">The list of VisumMatrix objects to dispose.</param>
    private static void DisposeMatrices(List<VisumMatrix> matrices)
    {
        foreach (var matrix in matrices)
        {
            matrix.Dispose();
        }
        matrices.Clear();
    }
}
