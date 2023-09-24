namespace TMG.Visum.RoadAssignment;

/// <summary>
/// The different type of matrices that can be skimmed
/// </summary>
/// <seealso cref="https://cgi.ptvgroup.com/vision-help/VISUM_2023_ENG/Content/1_Benutzermodell_IV/1_5_Kenngroessen_des_IV.htm"/>
public enum PrTLosTypes
{
    /// <summary>
    /// TSys-specific travel time t0 in unloaded network
    /// </summary>
    T0,

    /// <summary>
    /// TSys-specific travel time tCur in loaded network
    /// </summary>
    TCur,

    /// <summary>
    /// TSys-specific speed v0 in unloaded network
    /// </summary>
    V0,

    /// <summary>
    /// TSys-specific speed vCur in loaded network
    /// </summary>
    VCur,

    /// <summary>
    /// TSys-specific impedance in unloaded network
    /// </summary>
    Impedance,

    /// <summary>
    /// Distance covered from origin to destination
    /// </summary>
    TripDistance,

    /// <summary>
    /// Distance covered from origin to destination
    /// </summary>
    DirectDistance,

    /// <summary>
    /// Sum of AddValue 1
    /// </summary>
    AddValue1,

    /// <summary>
    /// Sum of AddValue 2
    /// </summary>
    AddValue2,

    /// <summary>
    /// Sum of AddValue 3
    /// </summary>
    AddValue3,

    /// <summary>
    /// Sum of TSys-AddValue data
    /// </summary>
    AddValueTSys,

    /// <summary>
    /// Toll of traversed links
    /// </summary>
    Toll,

    /// <summary>
    /// Flexible calculation of a mean attribute value per OD pair, also allows for the linkage
    /// of attributes of different traversed network objects
    /// </summary>
    UserDefined,

}

/// <summary>
/// Provides internal functions for working with PrTLoSTypes
/// </summary>
internal static class PrTLoSTypesHelper
{
    /// <summary>
    /// Used for getting the name when building a SINGLESKIMMATRIXPARA
    /// </summary>
    /// <param name="type">The type to get the calculation name from.</param>
    /// <returns>The string to be used when calculating the skim</returns>
    /// <exception cref="VisumException">Thrown if the type is not valid.</exception>
    public static string GetCalculationName(this PrTLosTypes type)
    {
        return type switch
        {
            PrTLosTypes.T0 => "T0",
            PrTLosTypes.TCur => "TCUR",
            PrTLosTypes.V0 => "V0",
            PrTLosTypes.VCur => "VCUR",
            PrTLosTypes.Impedance => "IMPEDANCE",
            PrTLosTypes.TripDistance => "TRIPDIST",
            PrTLosTypes.DirectDistance => "DIRECTDIST",
            PrTLosTypes.AddValue1 => "ADDVAL1",
            PrTLosTypes.AddValue2 => "ADDVAL2",
            PrTLosTypes.AddValue3 => "ADDVAL3",
            PrTLosTypes.AddValueTSys => "ADDVAL_TSYS",
            PrTLosTypes.Toll => "TOLL",
            PrTLosTypes.UserDefined => "USERDEF",
            _ => throw new VisumException("Undefined PrTLoSType!")
        };
    }

    /// <summary>
    /// Computes the resulting matrix name given the demand segment
    /// </summary>
    /// <param name="type">The type of skim matrix.</param>
    /// <param name="demandSegment">The demand segment used.</param>
    /// <returns>Returns the expected name of the matrix after being calculated.</returns>
    public static string GetMatrixName(this PrTLosTypes type, VisumDemandSegment demandSegment)
    {
        // We add a space at the end of each typeName to reduce the allocations.
        var typeName = type switch
        {
            PrTLosTypes.T0 => "t0 ",
            PrTLosTypes.TCur => "tCur ",
            PrTLosTypes.V0 => "v0 ",
            PrTLosTypes.VCur => "vCur ",
            PrTLosTypes.Impedance => "Impedance ",
            PrTLosTypes.TripDistance => "Trip distance ",
            PrTLosTypes.DirectDistance => "Direct distance ",
            PrTLosTypes.AddValue1 => "AddValue 1 ",
            PrTLosTypes.AddValue2 => "AddValue 2 ",
            PrTLosTypes.AddValue3 => "AddValue 3 ",
            PrTLosTypes.AddValueTSys => "AddValue-Tsys ",
            PrTLosTypes.Toll => "Toll ",
            PrTLosTypes.UserDefined => "User-defined ",
            _ => throw new VisumException("Undefined PrTLoSType!")
        };
        return typeName + demandSegment.Code;
    }

}
