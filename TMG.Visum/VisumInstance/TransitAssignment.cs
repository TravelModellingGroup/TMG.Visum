// Ignore Spelling: Visum

using CommunityToolkit.HighPerformance;
using TMG.Visum.TransitAssignment;

namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Execute a transit assignment using a single demand segment
    /// </summary>
    /// <param name="segment">The demand segment to execute with.</param>
    /// <param name="loSToGenerate"></param>
    /// <param name="parameters">The parameters to use for the transit assignment.</param>
    /// <param name="iterations">
    ///     The number of iterations to execute, this only makes sense if you
    ///     are also using Surface Transit Speed Updating.
    /// </param>
    /// <param name="stsuParmaters">Parameters for Surface Transit Speed Updating.</param>
    /// <returns>A list of matrices for the demand segment for each LoS to Generate.</returns>
    /// <exception cref="VisumException"></exception>
    public List<VisumMatrix> ExecuteTransitAssignment(VisumDemandSegment segment, IList<PutLoSTypes> loSToGenerate,
        TransitAlgorithmParameters parameters, int iterations = 1, IList<STSUParameters>? stsuParmaters = null)
    {
        return ExecuteTransitAssignment(new VisumDemandSegment[] { segment }, loSToGenerate, parameters)[0];
    }

    /// <summary>
    /// Execute a transit assignment using multiple demand segments.
    /// </summary>
    /// <param name="segments">The demand segments to execute with.</param>
    /// <param name="loSToGenerate"></param>
    /// <param name="parameters">The parameters to use for the transit assignment.</param>
    /// <param name="iterations">
    ///     The number of iterations to execute, this only makes sense if you
    ///     are also using Surface Transit Speed Updating.
    /// </param>
    /// <param name="stsuParmaters">Parameters for Surface Transit Speed Updating.</param>
    /// <returns>A list for each demand segment of all of the requested LoS matrices.</returns>
    public List<List<VisumMatrix>> ExecuteTransitAssignment(IList<VisumDemandSegment> segments, IList<PutLoSTypes> loSToGenerate,
        TransitAlgorithmParameters parameters, int iterations = 1, IList<STSUParameters>? stsuParmaters = null)
    {
        CheckTransitAssignmentParameters(segments);

        _lock.EnterWriteLock();
        VisumStandardTimeSeries? tempTimeSeries = null;
        try
        {
            tempTimeSeries = SetupTempTimeSeries(segments, parameters);
            UpdateSTSUSegmentSpeeds(stsuParmaters);
            List<List<VisumMatrix>>? matrices = null;
            for (int i = 0; i < iterations; i++)
            {
                // Only generate the LoS during the last iteration
                matrices = ExecuteTransitAssignment(segments,
                    i < iterations - 1 ? Array.Empty<PutLoSTypes>() : loSToGenerate,
                    parameters);
                UpdateDwellTimes(stsuParmaters);
            }
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

    private void UpdateSTSUSegmentSpeeds(IList<STSUParameters>? stsuParmaters)
    {
        if (stsuParmaters is null || stsuParmaters.Count <= 0)
        {
            return;
        }
        ObjectDisposedException.ThrowIf(_visum is null, this);
        var filter = _visum.Filters.LineGroupFilter().TimeProfileItemFilter();
        foreach (var parameter in stsuParmaters)
        {

        }
        filter.UseFilter = true;
        var linkTimeAttribute = $"TCUR_PRTSYS({stsuParmaters[0].AutoDemandSegment})";
        // Update the 
        foreach (ITimeProfile timeProfile in _visum.Net.TimeProfiles)
        {
            // Find the stsuParameter to use for this time profile
            int index = -1;
            if (index >= 0)
            {
                var autoCorrelation = stsuParmaters[index].AutoCorrelation;
                foreach (ITimeProfileItem item in timeProfile.TimeProfileItems)
                {
                    if (!item.Active)
                    {
                        continue;
                    }
                    double segmentTime = (double)(((ILink)item.LineRouteItem.AttValue["INLINK"]).AttValue[linkTimeAttribute]) * autoCorrelation;
                    item.AttValue["ADDVALUE"] = segmentTime;
                }
                timeProfile.Active = true;
            }
            else
            {
                timeProfile.Active = false;
            }
        }

        // Step 2: Assign the updated run-times
        ExecuteSetRunAndDwellTimesInternal(new SetRunAndDwellTimeParameters()
        {
            AddValues = false,
            UpdateRunTime = true,
            RunTimeLinkFactor = 1.0f,
            RunTimeLinkAttrId = $"AddValue",
            RunTimeGuardOnlyActiveLinks = true,
        });


    }

    private void UpdateDwellTimes(IList<STSUParameters>? stsuParmaters)
    {
        if (stsuParmaters is null)
        {
            return;
        }

        foreach (var parameters in stsuParmaters)
        {

        }
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

}
