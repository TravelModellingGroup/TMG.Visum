using TMG.Visum.TransitAssignment;

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

    [RunParameter("Use Stored Headways", false, "Use the headways stored in the HeadwayAttribute instead of computing it.")]
    public bool UseStoredHeadways;

    [RunParameter("Headway Attribute", "", "An attribute for either saving headways to, or to read from.")]
    public string HeadwayAttribute = string.Empty;

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
            WalkTimeValue = WalkTimeValue,
            HeadwayAttribute = HeadwayAttribute,
            UseStoredHeadways = UseStoredHeadways,       
        };
    }

    public override bool RuntimeValidation(ref string? error)
    {
        if(UseStoredHeadways && string.IsNullOrWhiteSpace(HeadwayAttribute))
        {
            error = "If you want to use the stored headways you must also include the name of the Headway Attribute!";
            return false;
        }
        return base.RuntimeValidation(ref error);
    }
}
