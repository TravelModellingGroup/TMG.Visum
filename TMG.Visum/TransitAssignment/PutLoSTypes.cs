using TMG.Visum.RoadAssignment;

namespace TMG.Visum.TransitAssignment;

/// <summary>
/// 
/// </summary>
/// <seealso cref="https://cgi.ptvgroup.com/vision-help/VISUM_2023_ENG/Content/1_Benutzermodell_OeV/1_6_Kenngroessen_der_Zeit.htm"/>
public enum PutLoSTypes
{
    #region TimeMatrices

    /// <summary>
    /// Time required for covering the origin connector
    /// </summary>
    AccessTime = 0,

    /// <summary>
    /// Time required for covering the destination connector
    /// </summary>
    EgressTime = 1,

    /// <summary>
    /// Time in a sharing vehicle The values are
    /// averaged over all public transport paths,
    /// not only the vehicle sharing paths, of an OD pair.
    /// </summary>
    SharingTravelTime = 2,

    /// <summary>
    /// Time spent on renting the vehicle. The values are averaged over all
    /// public transport paths, not only the vehicle sharing paths, of an OD pair.
    /// </summary>
    SharingRentalProcessTime = 3,

    /// <summary>
    /// Time spent on returning the vehicle The values are
    /// averaged over all public transport paths, not only
    /// the vehicle sharing paths, of an OD pair.
    /// </summary>
    SharingReturnProcessTime = 4,

    /// <summary>
    /// Time in DRT vehicle including the time required for entry and exit.
    /// It does not contain the wait time at the pickup node. 
    /// The values are averaged over all PuT paths of an OD pair.	
    /// </summary>
    DRTTime = 5,

    /// <summary>
    /// Time the passenger waits for the arrival of the DRT vehicle This skim refers only to DRT
    /// path legs and is not averaged over the entire public transport demand on the relation.
    /// </summary>
    DRTWaitTime = 6,

    /// <summary>
    /// Wait time at the start stop point (applies to the headway-based assignment only,
    /// as for the timetable-based procedure OWT = 0 is assumed)
    /// </summary>
    OriginWaitTime = 7,

    /// <summary>
    /// Product from the origin wait time and the weighting factor of the origin wait time
    /// in the settings for the impedance of the headway-based assignment.
    /// This skim is only available in the headway-based assignment.
    /// </summary>
    WeightedOriginWaitTime = 8,

    /// <summary>
    /// Wait time between arrival and departure at a transfer stop point
    /// </summary>
    TransferWaitTime = 9,

    /// <summary>
    /// Product from the transfer wait time and the weighting factor of the
    /// transfer wait time in the settings for the impedance of the
    /// headway-based assignment. This skim is only available in the headway-based assignment.
    /// </summary>
    WeightedTransferWaitTime = 10,

    /// <summary>
    /// Extended wait time according to the settings for the transfer
    /// wait time in the perceived journey time definition for the timetable-based assignment.
    /// </summary>
    ExtendedTransferWaitTime = 11,

    /// <summary>
    // Time spent inside PuT vehicles including dwell times at stops.
    /// </summary>
    InVehicleTime = 12,

    /// <summary>
    /// Time spent inside PuT vehicles of a certain public transport system.
    /// </summary>
    InVehicleTimeByTSys = 13,

    /// <summary>
    /// Run time with PuT Aux transport systems
    /// </summary>
    PuTAuxTime = 14,

    /// <summary>
    /// Walk time for transfer links between two stop points within
    /// a stop area or between different stop areas of a stop and on links in the network
    /// </summary>
    WalkTime = 15,

    /// <summary>
    /// Time between the departure from the origin zone and the arrival at the destination zone
    /// JRT = ACT + OWT + ∑ IVT + ∑ TWT + ∑ WKT + EGT
    /// </summary>
    JourneyTime = 16,

    /// <summary>
    /// Time between the departure from the origin stop point and the arrival at the destination stop point
    /// RIT = ∑ IVT + ∑ TWT + ∑ WKT
    /// </summary>
    RideTime = 17,

    /// <summary>
    /// PJT = f(ACT, EGT, OWT, TWT, NTR, IVT, WKT, XZ)
    /// </summary>
    PerceivedJourneyTime = 18,

    /// <summary>
    /// Difference delta T between the desired departure time (desired arrival time) 
    /// and the actual departure time (arrival time) using a departure-based (arrival-based) time reference.
    /// </summary>
    AdaptionTime = 19,

    /// <summary>
    /// Variant of the adaptation time which assumes that the entire demand
    /// of each time interval is assigned to the connection with the minimum impedance.
    /// </summary>
    ExtendedAdaptionTime = 20,

    /// <summary>
    /// Expected extension of the connection duration calculated per person
    /// in minutes. Based on previously calculated PuT paths of the 
    /// assignment and its rounded results.
    /// </summary>
    RiskOfDelayPerPerson = 21,
    #endregion

    #region DistanceMatrices
    /// <summary>
    /// Length of access path on origin connector
    /// </summary>
    AccessDistance = 22,

    /// <summary>
    /// Length of egress path on origin connector
    /// </summary>
    EgressDistance = 23,

    /// <summary>
    /// Distance covered in vehicle without transfer walk links
    /// </summary>
    InVehicleDistance = 24,

    /// <summary>
    /// Travel distance inside vehicles of a specific public transport system
    /// </summary>
    InVehicleDistancePerTSys = 25,

    /// <summary>
    /// Travel distance in Sharing vehicle
    /// </summary>
    SharingTravelDistance = 26,

    /// <summary>
    /// Distance travelled with DRT vehicles
    /// This skim refers exclusively to DRT path legs and is not averaged over the entire public transport demand on the relation.
    /// </summary>
    DRTDistance = 27,

    /// <summary>
    /// In-vehicle distance for a PuT-Aux transport system
    /// </summary>
    PuTAuxDistance = 28,

    /// <summary>
    /// Length of a transfer link between the two stop points
    /// </summary>
    WalkDistance = 29,

    /// <summary>
    /// Distance covered between origin and destination zone
    /// Journey distance = Access distance + In-vehicle distance + Walk distance + Egress distance
    /// </summary>
    JourneyDistance = 30,

    /// <summary>
    /// Covered distance from origin stop point to destination stop point
    /// Ride distance = In - vehicle distance + Walk distance
    /// </summary>
    RideDistance = 31,

    /// <summary>
    /// Direct distance between origin and destination zone
    /// </summary>
    DirectDistance = 32,

    #endregion

    #region MonetarySkims

    /// <summary>
    /// Fare for the PuT ride between origin and destination zone
    /// </summary>
    Fare = 33,

    #endregion

    #region DerivedSkims

    /// <summary>
    /// Impedance of a connection = f (perceived journey time, fare, temporal utility). 
    /// For the skim matrix you can select whether the temporal component should flow into 
    /// the impedance in minutes or seconds.
    /// </summary>
    ImpedanceInaTimeInterval = 34,

    /// <summary>
    /// Logsum of impedance. Can only be selected for timetable-based assignment and 
    /// in conjunction with the "Logit" choice model. This skim is an alternative 
    /// aggregation function for the "impedance” skim. 
    /// </summary>
    ImpedanceLogSum = 35,

    /// <summary>
    /// Ratio of the journey distance and the journey time between
    /// origin and destination zone [km/h]
    /// </summary>
    JourneySpeed = 36,

    /// <summary>
    /// Ratio of the direct distance and the journey time between 
    /// origin and destination zone [km/h]
    /// </summary>
    DirectDistanceSpeed = 37,

    /// <summary>
    /// Distance covered in the TSys as a percentage of the total in-vehicle
    /// distance of the connection
    /// </summary>
    InVehicleDistanceAAsPercentageByTSys = 38,

    /// <summary>
    /// Skim which results from a user-defined formula according to the set parameters.
    /// The unit of the journey time equivalent of the calculated skims 
    /// is determined by the user-defined formula.
    /// </summary>
    EquivalentJourneyTime = 39,

    /// <summary>
    /// The extended impedance is a component of the perceived journey time (PJT).
    /// It can be defined in the settings for the impedance of the 
    /// timetable-based assignment and is thus only available in the timetable-based assignment.
    /// </summary>
    ExtendedImpedance = 40,

    /// <summary>
    /// https://cgi.ptvgroup.com/vision-help/VISUM_2024_ENG/Content/1_Benutzermodell_OeV/1_6_Abgeleitete_Kenngroessen.htm
    /// </summary>
    Utility = 41,

    /// <summary>
    /// Time during which a passenger has no seat in the course of this journey.
    /// The skim is calculated as journey time weighted by vehicle journey item.
    /// Its weight is a function of the volume/seat capacity ratio.
    /// </summary>
    DiscomfortDueToCapacityOverload = 42,

    /// <summary>
    /// Share of the passengers who are likely to experience a delay exceeding a specified threshold.
    /// </summary>
    ShareOfODTripsWithRelevantDelay = 43,

    /// <summary>
    /// Share of passengers denied boarding due capacity overload of vehicles
    /// </summary>
    ShareOfFailToBoard = 44,

    /// <summary>
    /// The risk for each person of not being able to board a relation is calculated
    /// as a weighted total of all transfers and the risk at the boarding stop.
    /// </summary>
    RiskOfFailToBoardPerPerson = 45,

    #endregion

    #region Frequency

    /// <summary>
    /// Number of transfers between origin and destination stop point (per connection).
    /// </summary>
    NumberOfTransfers = 46,

    /// <summary>
    /// The service frequency indicates how often a relation is traversed. A flow problem 
    /// is solved on the graph of all determined routes. Service frequency thus depends 
    /// on the "weakest" part in the transport supply. The parameter Number of 
    /// arrival time points shows the definition valid until PTV Visum 2020.
    /// </summary>
    ServiceFrequency = 47,

    /// <summary>
    /// Number of transfers with different operators of previous and next path leg.
    /// </summary>
    NumberOfOperatorChanges = 48,

    /// <summary>
    /// Number of traversed fare zones. The skim depends of the ticket type(s) 
    /// used for the connection and returns zero if no zone-based ticket type is used.
    /// </summary>
    NumberOfFareZones = 49,

    /// <summary>
    /// The service frequency indicates the number of times a relation is traversed. 
    /// For the timetable-based assignment, the service frequency is defined as the number 
    /// of different arrival time points for connections departing in the assignment period 
    /// and in the post-assignment time period, but before a possible second occurrence of the 
    /// start of the assignment period. The latter means in particular that the post-assignment 
    /// time period is not taken into account if you do not use a calendar and define an assignment 
    /// interval from 0:00 to 24:00. To the headway-based assignment, the following 
    /// applies: On the graph of all determined routes, a flow problem is solved. 
    /// Service frequency thus depends on the "weakest" part in the transport supply.
    /// </summary>
    NumberOfArrivalTimePoints = 50,

    #endregion

    #region AttributeData

    /// <summary>
    /// Throughout the entire path aggregated value of the selected
    /// (direct or indirect) path leg attribute, for example Line route\AddValue1
    /// </summary>
    PathLegAttribute = 51,

    #endregion
}

internal static class PuTLoSTypesHelper
{
    /// <summary>
    /// Get the converted name for the result matrices for
    /// a headway based assignment.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="VisumException">
    /// Thrown if the matrix is not a valid result for a headway based assignment.
    /// </exception>
    public static string GetCalculationName(this PutLoSTypes type)
    {
        return type switch
        {
            PutLoSTypes.AccessDistance => "ACCESSDIST",
            PutLoSTypes.AccessTime => "ACCESSTIME",
            PutLoSTypes.DirectDistance => "DIRECTDIST",
            PutLoSTypes.DirectDistanceSpeed => "DIRECTDISTSPEED",
            PutLoSTypes.EgressDistance => "EGRESSDIST",
            PutLoSTypes.EgressTime => "EGRESSTIME",
            PutLoSTypes.EquivalentJourneyTime => "EQUIVALENTJOURNEYTIME",
            PutLoSTypes.Fare => "FARE",
            PutLoSTypes.ImpedanceInaTimeInterval => "IMPEDANCE",
            PutLoSTypes.InVehicleDistance => "INVEHDIST",
            PutLoSTypes.InVehicleDistancePerTSys => "INVEHDISTBYTSYS(B)",
            PutLoSTypes.InVehicleDistanceAAsPercentageByTSys => "INVEHDISTPERCBYTSYS(B)",
            PutLoSTypes.InVehicleTime => "INVEHTIME",
            PutLoSTypes.InVehicleTimeByTSys => "INVEHTIMEBYTSYS(B)",
            PutLoSTypes.JourneyDistance => "JOURNEYDIST",
            PutLoSTypes.JourneySpeed => "JOURNEYSPEED",
            PutLoSTypes.JourneyTime => "JOURNEYTIME",
            PutLoSTypes.NumberOfFareZones => "NUMFZ",
            PutLoSTypes.NumberOfTransfers => "NUMTRANSFERS",
            PutLoSTypes.OriginWaitTime => "ORIGINWAITTIME",
            PutLoSTypes.PathLegAttribute => "PATHLEGATTRIBUTE",
            PutLoSTypes.PerceivedJourneyTime => "PERCEIVEDJOURNEYTIME",
            PutLoSTypes.PuTAuxDistance => "PUTAUXDIST",
            PutLoSTypes.PuTAuxTime => "PUTAUXTIME",
            PutLoSTypes.RideDistance => "RIDEDIST",
            PutLoSTypes.RideTime => "RIDETIME",
            PutLoSTypes.ServiceFrequency => "SERVICEFREQUENCY",
            PutLoSTypes.TransferWaitTime => "TRANSFERWAITTIME",
            PutLoSTypes.WalkDistance => "WALKDIST",
            PutLoSTypes.WalkTime => "WALKTIME",
            PutLoSTypes.WeightedOriginWaitTime => "WEIGHTEDORIGINWAITTIME",
            PutLoSTypes.WeightedTransferWaitTime => "WEIGHTEDTRANSFERWAITTIME",
            _ => throw new VisumException("Undefined PuTLoSType!")
        };
    }

    /// <summary>
    /// Get the name of a matrix that would be generated given the type and the demand segment.
    /// </summary>
    /// <param name="loSType">The type of LoS.</param>
    /// <param name="demandSegment">The demand segment.</param>
    /// <returns>The expected name of the skim matrix.</returns>
    /// <exception cref="VisumException">Thrown if the LoS is of an unknown type.</exception>
    public static string GetMatrixName(this PutLoSTypes loSType, VisumDemandSegment demandSegment)
    {
        // We add a space at the end of each typeName to reduce the allocations.
        var typeName = loSType switch
        {
            PutLoSTypes.AccessTime => "Access time",
            PutLoSTypes.EgressTime => "Egress time",
            PutLoSTypes.AccessDistance => "Access distance",
            PutLoSTypes.DirectDistance => "Direct distance",
            PutLoSTypes.DirectDistanceSpeed => "Direct distance speed",
            PutLoSTypes.EgressDistance => "Egress distance",
            PutLoSTypes.EquivalentJourneyTime => "Equivalent journey time",
            PutLoSTypes.Fare => "Fare",
            PutLoSTypes.ImpedanceInaTimeInterval => "Impedance",
            PutLoSTypes.InVehicleDistance => "In-vehicle distance",
            PutLoSTypes.InVehicleDistancePerTSys => "INVEHDISTBYTSYS(B)",
            PutLoSTypes.InVehicleDistanceAAsPercentageByTSys => "In-vehicle distance-TSys [%]",
            PutLoSTypes.InVehicleTime => "In-vehicle time",
            PutLoSTypes.InVehicleTimeByTSys => "In-vehicle time-TSys",
            PutLoSTypes.JourneyDistance => "Journey distance",
            PutLoSTypes.JourneySpeed => "Journey speed",
            PutLoSTypes.JourneyTime => "Journey time",
            PutLoSTypes.NumberOfFareZones => "Number of fare zones",
            PutLoSTypes.NumberOfTransfers => "Number of transfers",
            PutLoSTypes.OriginWaitTime => "Origin wait time",
            PutLoSTypes.PathLegAttribute => "PATHLEGATTRIBUTE",
            PutLoSTypes.PerceivedJourneyTime => "Perceived journey time",
            PutLoSTypes.PuTAuxDistance => "PuT Aux distance",
            PutLoSTypes.PuTAuxTime => "PuT Aux time",
            PutLoSTypes.RideDistance => "Ride distance",
            PutLoSTypes.RideTime => "Ride time",
            PutLoSTypes.ServiceFrequency => "Service frequency",
            PutLoSTypes.TransferWaitTime => "Transfer wait time",
            PutLoSTypes.WalkDistance => "Walk distance",
            PutLoSTypes.WalkTime => "Walk time",
            PutLoSTypes.WeightedOriginWaitTime => "Weighted origin wait time",
            PutLoSTypes.WeightedTransferWaitTime => "Weighted transfer wait time",
            _ => throw new VisumException("Unknown PuTLoSType!")
        };
        return typeName + " " + demandSegment.Code;
    }

}
