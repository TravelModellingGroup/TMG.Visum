using TMG.Visum.TransitAssignment;
using static TMG.Visum.Assign.AssignTransitTool;

namespace TMG.Visum.Assign.TransitAssignmentAlgorithm;

[ModuleInformation(Description = "Provides parameters to control the Headway PutAssignment algorithm.")]
public sealed class HeadwayAlgorithm : TransitAssignmentAlgorithmModule
{
    [RunParameter("AccessTimeVal", 1.0f, "")]
    public float AccessTimeVal;

    [RunParameter("BoardingPenaltyPuTAttribute", "", "")]
    public string BoardingPenaltyPuTAttribute = null!;

    [RunParameter("BoardingPenaltyPuTAuxAttribute", "", "")]
    public string BoardingPenaltyPuTAuxAttribute = null!;

    [RunParameter("EgressTimeVal", 1.0f, "")]
    public float EgressTimeVal;

    [RunParameter("FarePointVal", 0.0f, "")]
    public float FarePointVal;

    [RunParameter("InVehicleTimeVal", 1.0f, "")]
    public float InVehicleTimeVal;

    [RunParameter("InVehicleTimeWeightAttribute", "", "")]
    public string InVehicleTimeWeightAttribute = null!;

    [RunParameter("MeanDelayAttribute", "", "")]
    public string MeanDelayAttribute = null!;

    [RunParameter("NumberOfTransfersValue", 0.0f, "")]
    public float NumberOfTransfersValue;

    [RunParameter("OriginWaitTimeValue", 1.0f, "")]
    public float OriginWaitTimeValue;

    [RunParameter("PerceivedJourneyTimeValue", 1.0f, "")]
    public float PerceivedJourneyTimeValue;

    [RunParameter("PublicTransitAuxiliaryTimeValue", 1.0f, "")]
    public float PublicTransitAuxiliaryTimeValue;

    [RunParameter("TransferWaitTimeValue", 1.0f, "")]
    public float TransferWaitTimeValue;

    [RunParameter("TransferWaitTimeWeightAttribute", "", "")]
    public string TransferWaitTimeWeightAttribute = null!;

    [RunParameter("UseFareModel", true, "")]
    public bool UseFareModel;

    [RunParameter("WalkTimeValue", 1.0f, "")]
    public float WalkTimeValue;

    [RunParameter("Assignment Start Day Index", 1, "")]
    public int AssignmentStartDayIndex;

    [RunParameter("Assignment Start Time", "00:00:00", typeof(TimeOnly), "")]
    public TimeOnly AssignmentStartTime;

    [RunParameter("Assignment End Day Index", 2, "")]
    public int AssignmentEndDayIndex;

    [RunParameter("Assignment End Time", "00:00:00", typeof(TimeOnly), "")]
    public TimeOnly AssignmentEndTime;

    [RunParameter("Share Lower Bounds", 0.05f, "")]
    public float ShareLowerBounds;

    [RunParameter("Share Upper Bounds", 0.99f, "")]
    public float ShareUpperBounds;

    [RunParameter("Use Stored Headways", false, "Use the headways stored in the HeadwayAttribute instead of computing it.")]
    public bool UseStoredHeadways;

    [RunParameter("Headway Attribute", "", "An attribute for either saving headways to, or to read from.")]
    public string HeadwayAttribute = string.Empty;

    [RunParameter("Use Optimized Headways", true, "Should we use optimized headways? If not constant headways will be used.")]
    public bool UseOptimizedHeadways;

    [ModuleInformation(Description = "A module that describes how to update the speed of")]
    public class STSUClass : IModule
    {
        [RunParameter("Auto Demand Segment", "C", "The demand segment that is used for STSU to base its times off of.")]
        public string AutoDemandSegment = null!;

        [RunParameter("Boarding Duration", 1.9577, "The boarding duration in seconds per passenger to apply.")]
        public float BoardingDuration;

        [RunParameter("Alighting Duration", 1.1219, "The alighting duration in seconds per passenger to apply.")]
        public float AlightingDuration;

        [RunParameter("Default Duration", 7.4331, "The default duration in seconds per stop to apply.")]
        public float DefaultDuration;

        [RunParameter("Transit Auto Correlation", 1, "The multiplier to auto time to use to find transit time.")]
        public float Correlation;

        [RunParameter("Default EROW Speed", 20.0f, "The speed that transit lines will travel at that belong to this STSU Class.")]
        public float DefaultEROWSpeed;

        [SubModelInformation(Required = true, Description = "The filter used to select lines to apply the calculation to.")]
        public FileLocation FilterFile = null!;

        public bool RuntimeValidation(ref string? error)
        {
            if (DefaultEROWSpeed <= 0)
            {
                error = "The Default EROW Speed needs to be greater than zero!";
                return false;
            }
            if (DefaultDuration < 0)
            {
                error = "The Default Duration needs to be at least than zero!";
                return false;
            }
            if (BoardingDuration < 0)
            {
                error = "The Boarding Duration needs to be at least than zero!";
                return false;
            }
            if (AlightingDuration < 0)
            {
                error = "The Alighting Duration needs to be at least than zero!";
                return false;
            }
            return true;
        }

        public string Name { get; set; } = null!;

        public float Progress => 0f;

        public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);
    }

    [SubModelInformation(Required = false, Description = "The different surface transit speed updating classes.")]
    public STSUClass[] SurfaceTransitSpeedUpdating = null!;

    internal override TransitAlgorithmParameters GetTransitParameters()
    {
        return new HeadwayImpedanceParameters()
        {
            AssignmentStartTime = AssignmentStartTime,
            AssignmentStartDayIndex = AssignmentStartDayIndex,
            AssignmentEndTime = AssignmentEndTime,
            AssignmentEndDayIndex = AssignmentEndDayIndex,
            AccessTimeVal = AccessTimeVal,
            BoardingPenaltyPuTAttribute = BoardingPenaltyPuTAttribute,
            BoardingPenaltyPuTAuxAttribute = BoardingPenaltyPuTAuxAttribute,
            EgressTimeVal = EgressTimeVal,
            FarePointVal = FarePointVal,
            InVehicleTimeVal = InVehicleTimeVal,
            InVehicleTimeWeightAttribute = InVehicleTimeWeightAttribute,
            MeanDelayAttribute = MeanDelayAttribute,
            NumberOfTransfersValue = NumberOfTransfersValue,
            OriginWaitTimeValue = OriginWaitTimeValue,
            PerceivedJourneyTimeValue = PerceivedJourneyTimeValue,
            PublicTransitAuxiliaryTimeValue = PublicTransitAuxiliaryTimeValue,
            TransferWaitTimeValue = TransferWaitTimeValue,
            TransferWaitTimeWeightAttribute = TransferWaitTimeWeightAttribute,
            ShareLowerBounds = ShareLowerBounds,
            ShareUpperBounds = ShareUpperBounds,
            WalkTimeValue = WalkTimeValue,
            HeadwayAttribute = HeadwayAttribute,
            UseStoredHeadways = UseStoredHeadways,
            UseOptimizedHeadways = UseOptimizedHeadways,
            UseFareModel = UseFareModel,
            STSUParameters = CreateSTSUParameters()
        };
    }

    private STSUParameters[] CreateSTSUParameters()
    {
        if (SurfaceTransitSpeedUpdating.Length < 0)
        {
            return [];
        }

        return SurfaceTransitSpeedUpdating
            .Select(x => new STSUParameters()
            {
                AlightingDuration = x.AlightingDuration,
                AutoCorrelation = x.Correlation,
                AutoDemandSegment = x.AutoDemandSegment,
                BoardingDuration = x.BoardingDuration,
                DefaultEROWSpeed = x.DefaultEROWSpeed,
                StopDuration = x.DefaultDuration,
                FilterFileName = x.FilterFile.GetFilePath(),
            })
            .ToArray();
    }

    public override bool RuntimeValidation(ref string? error)
    {
        if (UseStoredHeadways && string.IsNullOrWhiteSpace(HeadwayAttribute))
        {
            error = "If you want to use the stored headways you must also include the name of the Headway Attribute!";
            return false;
        }
        return base.RuntimeValidation(ref error);
    }
}
