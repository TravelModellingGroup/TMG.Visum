using TMG.Visum.TransitAssignment;

namespace TMG.Visum.Assign.TransitAssignmentAlgorithm;

[ModuleInformation(Description = "Provides parameters to control the Headway PutAssignment algorithm.")]
public sealed class HeadwayAlgorithm : TransitAssignmentAlgorithmModule
{
    [RunParameter("AccessTimeVal", 0.0f, "")]
    public float AccessTimeVal;

    [RunParameter("BoardingPenaltyPuTAttribute", 0.0f, "")]
    public string BoardingPenaltyPuTAttribute = null!;

    [RunParameter("BoardingPenaltyPuTAuxAttribute", 0.0f, "")]
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

    internal override TransitAlgorithmParameters GetTransitParameters()
    {
        return new HeadwayImpedanceParameters()
        {
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
        };
    }
}
