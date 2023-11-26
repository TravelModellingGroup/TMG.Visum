
namespace TMG.Visum.Create;

[ModuleInformation(
    Description = "This module allows you to create or update new Demand Segments, Demand Time Series," +
    " and Standard Time Series."
    )]
public sealed class CreateSegmentsAndSeries : IVisumTool
{


    [ModuleInformation(
        Description = "Contains the information required to create or update a Standard Time Series."
        )]
    public class StandardTimeSeriesInfo : IModule
    {
        [RunParameter("Name", "Default", "The name for the Standard Time Series.", Index = 1)]
        public string TimeSeriesName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string Name { get; set; } = null!;

        public float Progress => 0f;

        public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

        internal void CreateOrUpdate(VisumInstance instance)
        {
            VisumStandardTimeSeries? series = null;
            try
            {
                if (!instance.TryGetStandardTimeSeries(TimeSeriesName, out series))
                {
                    series = instance.CreateStandardTimeSeries(TimeSeriesName, true);
                }
                // TODO: Fill in other values here if we get that far
            }
            finally
            {
                series?.Dispose();
            }
        }
    }

    [ModuleInformation(
    Description = "Contains the information required to create or update a Demand Time Series."
    )]
    public class DemandTimeSeriesInfo : IModule
    {
        [RunParameter("Code", "Default", "The Code for the Demand Time Series.", Index = 1)]
        public string Code = null!;

        [RunParameter("Name", "Default", "The name for the Demand Time Series.", Index = 2)]
        public string DemandTimeSeriesName = null!;

        [RunParameter("Standard Time Series Name", "Default", "The name of the Standard Time Series this demand time series will use.", Index = 3)]
        public string StandardTimeSeriesName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string Name { get; set; } = null!;

        public float Progress => 0f;

        public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

        internal void CreateOrUpdate(VisumInstance instance)
        {
            VisumDemandTimeSeries? series = null;
            VisumStandardTimeSeries? standardTimeSeries = null;
            try
            {
                if(!instance.TryGetStandardTimeSeries(StandardTimeSeriesName, out standardTimeSeries))
                {
                    throw new XTMFRuntimeException(this, $"Unable to find a Standard Time Series with the name {StandardTimeSeriesName}!");
                }
                if (!instance.TryGetDemandTimeSeries(Code, out series))
                {
                    instance.CreateDemandTimeSeries(Code, DemandTimeSeriesName, standardTimeSeries);
                }
                else
                {
                    series.Name = DemandTimeSeriesName;
                    series.StandardTimeSeries = standardTimeSeries;
                }
            }
            finally
            {
                standardTimeSeries?.Dispose();
                series?.Dispose();
            }
        }
    }

    [ModuleInformation(
        Description = "Contains the information required to create or update a Demand Segment."
        )]
    public class DemandSegmentInfo : IModule
    {
        [RunParameter("Code", "C", "The code for the demand segment.", Index = 0)]
        public string Code = null!;

        [RunParameter("Name", "C", "The name for the demand segment.", Index = 1)]
        public string SegmentName = null!;

        [RunParameter("Mode Code", "C", "The code for the mode that will use this demand segment.", Index = 2)]
        public string ModeCode = null!;

        [RunParameter("Demand Time Series Code", "Default", "The code of the Demand Time Series to use.", Index = 3)]
        public string DemandTimeSeriesCode = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string Name { get; set; } = null!;

        public float Progress => 0f;

        public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

        /// <summary>
        /// Create or update the Demand Segment
        /// </summary>
        /// <param name="instance">The VISUM instance to work within.</param>
        /// <exception cref="XTMFRuntimeException"><Thrown if a referenced mode or segment does not exist./exception>
        internal void CreateOrUpdate(VisumInstance instance)
        {
            VisumDemandSegment? visumDemandSegment = null;
            VisumMode? mode = null;
            VisumDemandTimeSeries? demandTimeSeries = null;
            try
            {
                if (!instance.TryGetMode(ModeCode, out mode))
                {
                    throw new XTMFRuntimeException(this, $"Unable to find a Mode with the code {ModeCode}!");
                }
                if (!instance.TryGetDemandTimeSeries(DemandTimeSeriesCode, out demandTimeSeries))
                {
                    throw new XTMFRuntimeException(this, $"Unable to find a Demand Time Series with the code {ModeCode}!");
                }
                if (!instance.TryGetDemandSegment(Code, out visumDemandSegment))
                {
                    // Create
                    visumDemandSegment = instance.CreateDemandSegment(Code, mode);
                }
                else
                {
                    visumDemandSegment.Mode = mode;
                }
                visumDemandSegment.DemandTimeSeries = demandTimeSeries;
            }
            finally
            {
                mode?.Dispose();
                demandTimeSeries?.Dispose();
                visumDemandSegment?.Dispose();
            }
        }
    }

    [SubModelInformation(Required = false, Description = "The standard time series to create or update", Index = 0)]
    public StandardTimeSeriesInfo[] StandardTimeSeries = null!;

    [SubModelInformation(Required = false, Description = "The demand time series to create or update", Index = 1)]
    public DemandTimeSeriesInfo[] DemandTimeSeries = null!;

    [SubModelInformation(Required = false, Description = "The demand segments to create or update", Index = 2)]
    public DemandSegmentInfo[] Segments = null!;

    public void Execute(VisumInstance instance)
    {
        foreach (var standardTimeSeries in StandardTimeSeries)
        {
            standardTimeSeries.CreateOrUpdate(instance);
        }
        foreach (var demandTimeSeries in DemandTimeSeries)
        {
            demandTimeSeries.CreateOrUpdate(instance);
        }
        foreach (var segment in Segments)
        {
            segment.CreateOrUpdate(instance);
        }
    }

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);
}
