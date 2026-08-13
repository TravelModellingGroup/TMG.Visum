using TMG.Visum.TransitAssignment;

namespace TMG.Visum.Assign.TransitAssignmentAlgorithm;

[Module(Description = "Provides parameters to control the Headway PutAssignment algorithm.", Name = "Headway Algorithm",
    DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Assign/TransitAssignmentAlgorithm/HeadwayAlgorithm.html"
    )]
public sealed class HeadwayAlgorithm : TransitAssignmentAlgorithmModule
{
    [Parameter(Name = "Access Time Value", DefaultValue = "1.0", Description = "", Index = 0)]
    public IFunction<float> AccessTimeVal = null!;

    [Parameter(Name = "Boarding Penalty PuT Attribute", DefaultValue = "", Description = "", Index = 1)]
    public IFunction<string> BoardingPenaltyPuTAttribute = null!;

    [Parameter(Name = "Boarding Penalty PuT Aux Attribute", DefaultValue = "", Description = "", Index = 2)]
    public IFunction<string> BoardingPenaltyPuTAuxAttribute = null!;

    [Parameter(Name = "Egress Time Value", DefaultValue = "1.0", Description = "", Index = 3)]
    public IFunction<float> EgressTimeVal = null!;

    [Parameter(Name = "Fare Point Value", DefaultValue = "0.0", Description = "", Index = 4)]
    public IFunction<float> FarePointVal = null!;

    [Parameter(Name = "In Vehicle Time Value", DefaultValue = "1.0", Description = "", Index = 5)]
    public IFunction<float> InVehicleTimeVal = null!;

    [Parameter(Name = "In Vehicle Time Weight Attribute", DefaultValue = "", Description = "", Index = 6)]
    public IFunction<string> InVehicleTimeWeightAttribute = null!;

    [Parameter(Name = "Mean Delay Attribute", DefaultValue = "", Description = "", Index = 7)]
    public IFunction<string> MeanDelayAttribute = null!;

    [Parameter(Name = "Number Of Transfers Value", DefaultValue = "0.0", Description = "", Index = 8)]
    public IFunction<float> NumberOfTransfersValue = null!;

    [Parameter(Name = "Origin Wait Time Value", DefaultValue = "1.0", Description = "", Index = 9)]
    public IFunction<float> OriginWaitTimeValue = null!;

    [Parameter(Name = "Perceived Journey Time Value", DefaultValue = "1.0", Description = "", Index = 10)]
    public IFunction<float> PerceivedJourneyTimeValue = null!;

    [Parameter(Name = "Public Transit Auxiliary Time Value", DefaultValue = "1.0", Description = "", Index = 11)]
    public IFunction<float> PublicTransitAuxiliaryTimeValue = null!;

    [Parameter(Name = "Transfer Wait Time Value", DefaultValue = "1.0", Description = "", Index = 12)]
    public IFunction<float> TransferWaitTimeValue = null!;

    [Parameter(Name = "Transfer Wait Time Weight Attribute", DefaultValue = "", Description = "", Index = 13)]
    public IFunction<string> TransferWaitTimeWeightAttribute = null!;

    [Parameter(Name = "Use Fare Model", DefaultValue = "true", Description = "", Index = 14)]
    public IFunction<bool> UseFareModel = null!;

    [Parameter(Name = "Walk Time Value", DefaultValue = "1.0", Description = "", Index = 15)]
    public IFunction<float> WalkTimeValue = null!;

    [Parameter(Name = "Assignment Start Day Index", DefaultValue = "1", Description = "", Index = 16)]
    public IFunction<int> AssignmentStartDayIndex = null!;

    [Parameter(Name = "Assignment Start Time", DefaultValue = "00:00:00", Description = "", Index = 17)]
    public IFunction<TimeOnly> AssignmentStartTime = null!;

    [Parameter(Name = "Assignment End Day Index", DefaultValue = "2", Description = "", Index = 18)]
    public IFunction<int> AssignmentEndDayIndex = null!;

    [Parameter(Name = "Assignment End Time", DefaultValue = "00:00:00", Description = "", Index = 19)]
    public IFunction<TimeOnly> AssignmentEndTime = null!;

    [Parameter(Name = "Share Lower Bounds", DefaultValue = "0.05", Description = "", Index = 20)]
    public IFunction<float> ShareLowerBounds = null!;

    [Parameter(Name = "Share Upper Bounds", DefaultValue = "0.99", Description = "", Index = 21)]
    public IFunction<float> ShareUpperBounds = null!;

    [Parameter(Name = "Use Stored Headways", DefaultValue = "false", Description = "Use the headways stored in the HeadwayAttribute instead of computing it.", Index = 22)]
    public IFunction<bool> UseStoredHeadways = null!;

    [Parameter(Name = "Headway Attribute", DefaultValue = "", Description = "An attribute for either saving headways to, or to read from.", Index = 23)]
    public IFunction<string> HeadwayAttribute = null!;

    [Parameter(Name = "Passenger Information", DefaultValue = "CompleteInformation", Description = "Which passenger information option should we use?", Index = 24)]
    public IFunction<HeadwayImpedanceParameters.HeadwayStrategy> PassengerInformation = null!;

    [Parameter(Name = "Precise Method Up To", DefaultValue = "30", Description = "The number of alternatives when using information before falling back to the approximation algorithm.", Index = 25)]
    public IFunction<int> PreciseMethodUpTo = null!;

    [Parameter(Name = "Approximation Iterations", DefaultValue = "100", Description = "The number of iterations to use when running the approximation algorithm if there are too many alternative paths.", Index = 26)]
    public IFunction<int> NumberOfIterationsUsingApproximation = null!;

    [Module(Description = "A module that describes how to update the speed of transit lines.", Name = "STSU Class", DocumentationLink = "https://tmg.utoronto.ca/doc/2.0/Visum/modules/Assign/TransitAssignmentAlgorithm/HeadwayAlgorithm.html")]
    public class STSUClass : IModule
    {
        [Parameter(Name = "Auto Demand Segment", DefaultValue = "C", Description = "The demand segment that is used for STSU to base its times off of.", Index = 0)]
        public IFunction<string> AutoDemandSegment = null!;

        [Parameter(Name = "Boarding Duration", DefaultValue = "1.9577", Description = "The boarding duration in seconds per passenger to apply.", Index = 1)]
        public IFunction<float> BoardingDuration = null!;

        [Parameter(Name = "Alighting Duration", DefaultValue = "1.1219", Description = "The alighting duration in seconds per passenger to apply.", Index = 2)]
        public IFunction<float> AlightingDuration = null!;

        [Parameter(Name = "Default Duration", DefaultValue = "7.4331", Description = "The default duration in seconds per stop to apply.", Index = 3)]
        public IFunction<float> DefaultDuration = null!;

        [Parameter(Name = "Transit Auto Correlation", DefaultValue = "1", Description = "The multiplier to auto time to use to find transit time.", Index = 4)]
        public IFunction<float> Correlation = null!;

        [Parameter(Name = "Default EROW Speed", DefaultValue = "20.0", Description = "The speed that transit lines will travel at that belong to this STSU Class.", Index = 5)]
        public IFunction<float> DefaultEROWSpeed = null!;

        [Parameter(Name = "Attribute For Bus Facility", DefaultValue = "", Description = "The name of the boolean Links attribute in Visum that indicated whether the link has an exclusive bus facility. Can be empty if no such attribute exists; in this case new links open only to buses must be used.", Index = 6)]
        public IFunction<string> ExclusiveBusFacilityAttribute = null!;

        [Parameter(Name = "Filter File", DefaultValue = "", Description = "The filter used to select lines to apply the calculation to.", Index = 7)]
        public IFunction<string> FilterFile = null!;

        public bool RuntimeValidation(ref string? error)
        {
            var defaultEROWSpeed = DefaultEROWSpeed.Invoke();
            if (defaultEROWSpeed <= 0)
            {
                error = "The Default EROW Speed needs to be greater than zero!";
                return false;
            }
            var defaultDuration = DefaultDuration.Invoke();
            if (defaultDuration < 0)
            {
                error = "The Default Duration needs to be at least than zero!";
                return false;
            }
            var boardingDuration = BoardingDuration.Invoke();
            if (boardingDuration < 0)
            {
                error = "The Boarding Duration needs to be at least than zero!";
                return false;
            }
            var alightingDuration = AlightingDuration.Invoke();
            if (alightingDuration < 0)
            {
                error = "The Alighting Duration needs to be at least than zero!";
                return false;
            }
            return true;
        }

        public string? Name { get; set; }
    }

    [SubModule(Name = "Surface Transit Speed Updating", Required = false, Description = "The different surface transit speed updating classes.", Index = 27)]
    public STSUClass[] SurfaceTransitSpeedUpdating = null!;

    internal override TransitAlgorithmParameters GetTransitParameters()
    {
        return new HeadwayImpedanceParameters()
        {
            AssignmentStartTime = AssignmentStartTime.Invoke(),
            AssignmentStartDayIndex = AssignmentStartDayIndex.Invoke(),
            AssignmentEndTime = AssignmentEndTime.Invoke(),
            AssignmentEndDayIndex = AssignmentEndDayIndex.Invoke(),
            AccessTimeVal = AccessTimeVal.Invoke(),
            BoardingPenaltyPuTAttribute = BoardingPenaltyPuTAttribute.Invoke(),
            BoardingPenaltyPuTAuxAttribute = BoardingPenaltyPuTAuxAttribute.Invoke(),
            EgressTimeVal = EgressTimeVal.Invoke(),
            FarePointVal = FarePointVal.Invoke(),
            InVehicleTimeVal = InVehicleTimeVal.Invoke(),
            InVehicleTimeWeightAttribute = InVehicleTimeWeightAttribute.Invoke(),
            MeanDelayAttribute = MeanDelayAttribute.Invoke(),
            NumberOfTransfersValue = NumberOfTransfersValue.Invoke(),
            OriginWaitTimeValue = OriginWaitTimeValue.Invoke(),
            PerceivedJourneyTimeValue = PerceivedJourneyTimeValue.Invoke(),
            PublicTransitAuxiliaryTimeValue = PublicTransitAuxiliaryTimeValue.Invoke(),
            TransferWaitTimeValue = TransferWaitTimeValue.Invoke(),
            TransferWaitTimeWeightAttribute = TransferWaitTimeWeightAttribute.Invoke(),
            ShareLowerBounds = ShareLowerBounds.Invoke(),
            ShareUpperBounds = ShareUpperBounds.Invoke(),
            WalkTimeValue = WalkTimeValue.Invoke(),
            HeadwayAttribute = HeadwayAttribute.Invoke(),
            UseStoredHeadways = UseStoredHeadways.Invoke(),
            PassengerInformation = PassengerInformation.Invoke(),
            UseFareModel = UseFareModel.Invoke(),
            STSUParameters = CreateSTSUParameters(),
            PreciseMethodUpTo = PreciseMethodUpTo.Invoke(),
            NumberOfIterationsUsingApproximation = NumberOfIterationsUsingApproximation.Invoke(),
        };
    }

    private STSUParameters[] CreateSTSUParameters()
    {
        if (SurfaceTransitSpeedUpdating == null || SurfaceTransitSpeedUpdating.Length < 0)
        {
            return [];
        }

        return SurfaceTransitSpeedUpdating
            .Select(x => new STSUParameters()
            {
                AlightingDuration = x.AlightingDuration.Invoke(),
                AutoCorrelation = x.Correlation.Invoke(),
                AutoDemandSegment = x.AutoDemandSegment.Invoke(),
                BoardingDuration = x.BoardingDuration.Invoke(),
                DefaultEROWSpeed = x.DefaultEROWSpeed.Invoke(),
                StopDuration = x.DefaultDuration.Invoke(),
                FilterFileName = Path.GetFullPath(x.FilterFile.Invoke()),
                BusFacilityAttributeName = x.ExclusiveBusFacilityAttribute.Invoke()
            })
            .ToArray();
    }

    public override bool RuntimeValidation(ref string? error)
    {
        var useStoredHeadways = UseStoredHeadways.Invoke();
        var headwayAttribute = HeadwayAttribute.Invoke();
        if (useStoredHeadways && string.IsNullOrWhiteSpace(headwayAttribute))
        {
            error = "If you want to use the stored headways you must also include the name of the Headway Attribute!";
            return false;
        }
        return base.RuntimeValidation(ref error);
    }
}
