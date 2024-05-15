// Ignore Spelling: Visum

using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using TMG.Visum.TransitAssignment;

namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Execute a transit assignment using a single demand segment
    /// </summary>
    /// <param name="segment">The demand segment to execute with.</param>
    /// <param name="loSToGenerate"></param>
    /// <param name="algorithm">The parameters to use for the transit assignment.</param>
    /// <param name="iterations">
    ///     The number of iterations to execute, this only makes sense if you
    ///     are also using Surface Transit Speed Updating.
    /// </param>
    /// <param name="stsuParmaters">Parameters for Surface Transit Speed Updating.</param>
    /// <returns>A list of matrices for the demand segment for each LoS to Generate.</returns>
    /// <exception cref="VisumException"></exception>
    public List<VisumMatrix> ExecuteTransitAssignment(VisumDemandSegment segment, IList<PutLoSTypes> loSToGenerate,
        TransitAlgorithmParameters algorithm, int iterations = 1)
    {
        return ExecuteTransitAssignment([segment], loSToGenerate, algorithm)[0];
    }

    /// <summary>
    /// Execute a transit assignment using multiple demand segments.
    /// </summary>
    /// <param name="segments">The demand segments to execute with.</param>
    /// <param name="loSToGenerate"></param>
    /// <param name="algorithm">The parameters to use for the transit assignment.</param>
    /// <param name="iterations">
    ///     The number of iterations to execute, this only makes sense if you
    ///     are also using Surface Transit Speed Updating.
    /// </param>
    /// <param name="stsuParmaters">Parameters for Surface Transit Speed Updating.</param>
    /// <returns>A list for each demand segment of all of the requested LoS matrices.</returns>
    public List<List<VisumMatrix>> ExecuteTransitAssignment(IList<VisumDemandSegment> segments, IList<PutLoSTypes> loSToGenerate,
        TransitAlgorithmParameters algorithm, int iterations = 1)
    {
        CheckTransitAssignmentParameters(segments);

        _lock.EnterWriteLock();
        try
        {
            VisumStandardTimeSeries? tempTimeSeries = SetupTempTimeSeries(segments, algorithm);
            algorithm.Setup(this);
            UpdateSTSUSegmentSpeeds(algorithm);
            List<List<VisumMatrix>>? matrices = null;
            for (int i = 0; i < iterations; i++)
            {
                var moreIterations = i < iterations - 1;
                // Only generate the LoS during the last iteration
                matrices = ExecuteTransitAssignment(segments,
                     moreIterations ? Array.Empty<PutLoSTypes>() : loSToGenerate,
                    algorithm);
                if (moreIterations)
                {
                    UpdateDwellTimes(algorithm);
                }
            }
            algorithm.CleanUp(this);
            // clean-up the temporary time series
            if (tempTimeSeries is not null)
            {
                var number = tempTimeSeries.Number;
                tempTimeSeries.Dispose();
                RemoveStandardTimeSeriesInternal(number);
            }
            return matrices!;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    private VisumStandardTimeSeries SetupTempTimeSeries(IList<VisumDemandSegment> segments, TransitAlgorithmParameters parameters)
    {
        // Create a new Time Series
        var tempTimeSeries = CreateStandardTimeSeriesInner("TempForTransitAssignment", false);
        var number = tempTimeSeries.Number;

        static int GetTime(int date, TimeOnly time)
        {
            return (date - 1) * 24 * 60 * 60 + time.Hour * 60 * 60 + time.Minute * 60 + time.Second;
        }

        var item = tempTimeSeries
            .GetWrappedObject()
            .AddTimeSeriesItem(GetTime(parameters.AssignmentStartDayIndex, parameters.AssignmentStartTime),
                               GetTime(parameters.AssignmentEndDayIndex, parameters.AssignmentEndTime));
        item.SetWeight(100.0);
        // Point all of the segments to the time series
        foreach (var segment in segments)
        {
            using var demandTimeSeries = segment.GetDemandTimeSeriesInternal();
            if (demandTimeSeries is not null)
            {
                demandTimeSeries.StandardTimeSeriesNumber = number;
            }
            else
            {
                throw new VisumException($"We did not have a demand time series for" +
                    $" transit segment '{segment.Name}'!");
            }
        }
        return tempTimeSeries;
    }

    private void UpdateSTSUSegmentSpeeds(TransitAlgorithmParameters algorithm)
    {
        ObjectDisposedException.ThrowIf(_visum is null, this);
        algorithm.UpdateSTSUSegmentSpeeds(this);
    }

    private void FilterOnlyActiveLines(TransitAlgorithmParameters algorithm)
    {
        var filter = _visum!.Filters.LineGroupFilter();
        // Clear the rest of the filters
        SetLineGroupFilterInternal(true);
        filter.LineFilter().UseFilter = false;
        filter.LineRouteFilter().UseFilter = false;
        filter.TimeProfileItemFilter().UseFilter = false;
        filter.LineRouteItemFilter().UseFilter = false;
        filter.VehJourneyFilter().UseFilter = false;
        filter.VehJourneyItemFilter().UseFilter = false;
        filter.TimeProfileFilter().UseFilter = false;
        algorithm.ApplyActiveLineFilter(filter);
    }

    /// <summary>
    /// Enable the line group filters
    /// MUST OWN WRITE LOCK
    /// </summary>
    /// <param name="enable">Should we enable or disable them?</param>
    internal void SetLineGroupFilterInternal(bool enable)
    {
        var filter = _visum!.Filters.LineGroupFilter();
        filter.UseFilterForLineRouteItems = enable;
        filter.UseFilterForLineRoutes = enable;
        filter.UseFilterForLines = enable;
        filter.UseFilterForTimeProfileItems = enable;
        filter.UseFilterForTimeProfiles = enable;
        filter.UseFilterForVehJourneyItems = enable;
        filter.UseFilterForVehJourneys = enable;
        filter.UseFilterForVehJourneySections = enable;
    }

    private void UpdateDwellTimes(TransitAlgorithmParameters algorithm)
    {
        algorithm.UpdateDwellTimes(this);
        SetLineGroupFilterInternal(false);
    }

    private List<List<VisumMatrix>> ExecuteTransitAssignment(IList<VisumDemandSegment> segments, IList<PutLoSTypes> loSToGenerate, TransitAlgorithmParameters parameters)
    {
        string? tempFileName = null;
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            if (!parameters.Validate(this, out var error))
            {
                throw new VisumException(error);
            }
            FilterOnlyActiveLines(parameters);
            ClearPreviousSkims(segments, loSToGenerate);
            tempFileName = WriteProcedure((writer) =>
            {
                writer.WriteStartElement("OPERATION");
                writer.WriteAttributeString("ACTIVE", "1");
                writer.WriteAttributeString("NO", "1");
                writer.WriteAttributeString("OPERATIONTYPE", "PuTAssignment");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                writer.WriteStartElement("PUTASSIGNMENTPARABASE");
                writer.WriteAttributeString("DSEGSET", string.Join(',', segments.Select(seg => seg.Code)));
                writer.WriteAttributeString("PUTASSIGNMENTVARIANT", parameters.VariantName);
                parameters.Write(writer, loSToGenerate);

                // end PUTASSIGNMENTPARABASE
                writer.WriteEndElement();
                // end OPERATION
                writer.WriteEndElement();
            });
            // Wipe out the previous procedures and run this.
            RunProceduresFromFileInternal(tempFileName);
            // Get the matrices that were returned
            var ret = new List<List<VisumMatrix>>();
            foreach (var demandSegment in segments)
            {
                var segmentMatrices = new List<VisumMatrix>();
                foreach (var loSType in loSToGenerate)
                {
                    var matrixName = loSType.GetMatrixName(demandSegment);
                    var matrix = GetMatrixByNameInner(matrixName);
                    segmentMatrices.Add(matrix);
                }
                ret.Add(segmentMatrices);
            }
            return ret;
        }
        catch (VisumException)
        {
            // Just pass through VisumExceptions
            throw;
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            Files.SafeDelete(tempFileName);
            SetLineGroupFilterInternal(false);
        }
    }

    private void ClearPreviousSkims(IList<VisumDemandSegment> segments, IList<PutLoSTypes> loSToGenerate)
    {
        foreach (var demandSegment in segments)
        {
            var segmentMatrices = new List<VisumMatrix>();
            foreach (var loSType in loSToGenerate)
            {
                var matrixName = loSType.GetMatrixName(demandSegment);
                if (TryGetMatrixInner(matrixName, out var matrix))
                {
                    int number = matrix.GetNumber();
                    DeleteMatrixInner(number);
                }
            }
        }
    }

    private static void CheckTransitAssignmentParameters(IList<VisumDemandSegment> segments)
    {
        if (!segments.Any())
        {
            throw new VisumException("There were no demand segments defined!");
        }
        if (segments.FirstOrDefault(seg => seg.DemandMatrix is null) is VisumDemandSegment noMatrix)
        {
            throw new VisumException($"The demand segment {noMatrix.Name} was not initialized with a demand matrix for assignment!");
        }

    }

    /// <summary>
    /// TESTING ONLY
    /// Call this function to test the results of a transit assignment that used STSU.
    /// </summary>
    /// <param name="autoCorrelation">The auto correlation factor used in the assignment.</param>
    /// <param name="defaultSpeed">The default speed used.</param>
    /// <param name="defaultStopDuration">The dwell time for each stop without considering boardings.</param>
    /// <param name="autoNetwork">The network to use for getting auto times.</param>
    /// <param name="error">If false, an error message describing the issue.</param>
    /// <returns>True if STSU passed all tests, false otherwise with a message.</returns>
    public bool TestSTSU(float autoCorrelation, float defaultSpeed,
        float defaultStopDuration, string autoNetwork, [NotNullWhen(false)] ref string? error)
    {

        /*
         * We can test if STSU is working by finding all of the links that each transit itinerary takes
         * and then sum up the travel time across the links.  We then need to sum the number of stops.
         */
        ObjectDisposedException.ThrowIf(_visum is null, _visum);
        var autoTimeAttriubte = $"TCUR_PRTSYS({autoNetwork})";
        const double maxLinkTime = 9999.0;
        const double maxDifference = 1.0 / 60.0;
        var timeProfiles = (object[])_visum.Net.TimeProfiles.GetAll;
        var links = _visum.Net.Links;
        HashSet<ILink> usedLinks = new();
        var any = false;
        foreach (ITimeProfile timeProfile in timeProfiles)
        {
            // Gather all of the links used
            var items = (object[])timeProfile.TimeProfileItems.GetAll;
            double totalLinkTime = 0.0;
            usedLinks.Clear();
            if(items.Length <= 0)
            {
                continue;
            }
            var lineRoute = ((ITimeProfileItem)items[0]).LineRouteItem.LineRoute;
            var lineRouteItems = lineRoute.LineRouteItems;
            var stsuDistance = 0.0;
            foreach (ILineRouteItem item in lineRouteItems)
            {
                var outLink = item.GetOutLink(_visum);
                if (outLink is not null
                    && !usedLinks.Contains(outLink))
                {
                    usedLinks.Add(outLink);
                    stsuDistance += outLink.GetLength();
                    try
                    {
                        var autoTime = (double)outLink.AttValue[autoTimeAttriubte];
                        // Check to see if the links is not traversal by auto
                        if (autoTime > maxLinkTime)
                        {
                            var length = outLink.GetLength();
                            autoTime = defaultSpeed * length;
                        }
                        totalLinkTime += Math.Floor(autoCorrelation * autoTime);
                        any = true;
                    }
                    catch
                    { }
                }
            }
            var stsuRunTime = totalLinkTime + defaultStopDuration * (items.Length - 2);
            var expectedRunTime = ((ITimeProfileItem)items[^1]).GetAccumulatedRunTime();
            var expectedDistance = ((ITimeProfileItem)items[^1]).GetAccumulatedRunDistance();
            if (Math.Abs(stsuDistance - expectedDistance) > maxDifference)
            {
                error = "Out of distance bounds!";
                return false;
            }
            if (Math.Abs(stsuRunTime - expectedRunTime) > maxDifference)
            {
                error = "Out of time bounds!";
                return false;
            }
        }
        if(!any)
        {
            error = "Not Any!";
            return false;
        }
        error = null;
        return true;
    }

}
