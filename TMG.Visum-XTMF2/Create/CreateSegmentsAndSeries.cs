
namespace TMG.Visum.Create;

[Module(
    Description = "This module allows you to create or update new Demand Segments, Demand Time Series," +
    " and Standard Time Series.",
    Name = "Create Segments and Series",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Create/CreateSegmentsAndSeries.html"
    )]
public sealed class CreateSegmentsAndSeries : BaseAction<VisumInstance>
{
    [Module(
        Description = "Contains the information required to create or update a Standard Time Series.",
        Name = "Standard Time Series Info",
        DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Create/CreateSegmentsAndSeries.html"
        )]
    public class StandardTimeSeriesInfo : IModule
    {
        [Parameter(Name = "Name", DefaultValue = "Default", Description = "The name for the Standard Time Series.", Index = 0)]
        public IFunction<string> TimeSeriesName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string? Name { get; set; } = null!;

        internal void CreateOrUpdate(VisumInstance instance)
        {
            VisumStandardTimeSeries? series = null;
            try
            {
                var name = TimeSeriesName.Invoke();
                if (!instance.TryGetStandardTimeSeries(name, out series))
                {
                    series = instance.CreateStandardTimeSeries(name, true);
                }
                // TODO: Fill in other values here if we get that far
            }
            finally
            {
                series?.Dispose();
            }
        }
    }

    [Module(
        Description = "Contains the information required to create or update a Demand Time Series.",
        Name = "Demand Time Series Info",
        DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Create/CreateSegmentsAndSeries.html"
    )]
    public class DemandTimeSeriesInfo : IModule
    {
        [Parameter(Name = "Code", DefaultValue = "Default", Description = "The Code for the Demand Time Series.", Index = 1)]
        public string Code = null!;

        [Parameter(Name = "Name", DefaultValue = "Default", Description = "The name for the Demand Time Series.", Index = 2)]
        public string DemandTimeSeriesName = null!;

        [Parameter(Name = "Standard Time Series Name", DefaultValue = "Default", Description = "The name of the Standard Time Series this demand time series will use.", Index = 3)]
        public string StandardTimeSeriesName = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string? Name { get; set; } = null!;

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

    [Module(
        Description = "Contains the information required to create or update a Demand Segment.",
        Name = "Demand Segment Info",
        DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Create/CreateSegmentsAndSeries.html"
        )]
    public class DemandSegmentInfo : IModule
    {
        [Parameter(Name = "Code", DefaultValue = "C", Description = "The code for the demand segment.", Index = 0)]
        public string Code = null!;

        [Parameter(Name = "Name", DefaultValue = "C", Description = "The name for the demand segment.", Index = 1)]
        public string SegmentName = null!;

        [Parameter(Name = "Mode Code", DefaultValue = "C", Description = "The code for the mode that will use this demand segment.", Index = 2)]
        public string ModeCode = null!;

        [Parameter(Name = "Demand Time Series Code", DefaultValue = "Default", Description = "The code of the Demand Time Series to use.", Index = 3)]
        public string DemandTimeSeriesCode = null!;

        public bool RuntimeValidation(ref string? error)
        {
            return true;
        }

        public string? Name { get; set; } = null!;

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

    [SubModule(Name = "Standard Time Series", Required = false, Description = "The standard time series to create or update", Index = 0)]
    public StandardTimeSeriesInfo[] StandardTimeSeries = null!;

    [SubModule(Name = "Demand Time Series", Required = false, Description = "The demand time series to create or update", Index = 1)]
    public DemandTimeSeriesInfo[] DemandTimeSeries = null!;

    [SubModule(Name = "Demand Segments", Required = false, Description = "The demand segments to create or update", Index = 2)]
    public DemandSegmentInfo[] Segments = null!;

    public override void Invoke(VisumInstance instance)
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

}
