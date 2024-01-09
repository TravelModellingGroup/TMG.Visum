// Ignore Spelling: Visum
using TMG.Visum.RoadAssignment;
using VISUMLIB;
namespace TMG.Visum;

public partial class VisumInstance
{
    /// <summary>
    /// Calculate a Road LoS attribute and generate a matrix.
    /// </summary>
    /// <param name="segment">The demand segment to process.</param>
    /// <param name="types">The different types of PrT skims to generate.</param>
    /// <exception cref="VisumException">Can throw an exception if VISUM has an issue when processing the command.</exception>
    public List<VisumMatrix> CalculateRoadLoS(VisumDemandSegment segment, List<RoadAssignment.PrTLosTypes> types)
    {
        string? tempFileName = null;
        try
        {
            _lock.EnterWriteLock();
            ObjectDisposedException.ThrowIf(_visum is null, this);
            ClearMatrices(segment, types);
            tempFileName = WriteProcedure((writer) =>
            {
                /*
                 <PRTSKIMMATRIXPARA DSEG="C" FILENAME="" FORMAT="V" ONLYRELATIONSWITHDEMAND="0"
							   SEARCHCRITERION="Impedance" SELECTODRELATIONTYPE="All" SEPARATOR="Blank"
							   SUMUPDESTCONNS="1" SUMUPLINKS="1" SUMUPORIGCONNS="1" SUMUPRESTRAFAREAS="1"
							   SUMUPTURNS="1" USEEXISTINGPATHS="1" WEIGHTING="Route Vol Avg">
                                    <SINGLESKIMMATRIXPARA CALCULATE="1" NAME="TCUR" SAVETOFILE="0"/>
                 */
                writer.WriteStartElement("OPERATION");
                writer.WriteAttributeString("ACTIVE", "1");
                writer.WriteAttributeString("NO", "1");
                writer.WriteAttributeString("OPERATIONTYPE", "PrTSkimMatrixCalculation");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                writer.WriteStartElement("PRTSKIMMATRIXPARA");
                writer.WriteAttributeString("DSEG", segment.Code);
                writer.WriteAttributeString("FORMAT", "V");
                writer.WriteAttributeString("ONLYRELATIONSWITHDEMAND", "0");
                writer.WriteAttributeString("SEARCHCRITERION", "Impedance");
                writer.WriteAttributeString("SELECTODRELATIONTYPE", "All");
                writer.WriteAttributeString("SEPARATOR", "Blank");
                writer.WriteAttributeString("SUMUPDESTCONNS", "1");
                writer.WriteAttributeString("SUMUPLINKS", "1");
                writer.WriteAttributeString("SUMUPORIGCONNS", "1");
                writer.WriteAttributeString("SUMUPRESTRAFAREAS", "1");
                writer.WriteAttributeString("SUMUPTURNS", "1");
                writer.WriteAttributeString("USEEXISTINGPATHS", "1");
                writer.WriteAttributeString("WEIGHTING", "Route Vol Avg");

                // Specify the particular matrix to compute
                foreach (var type in types)
                {
                    writer.WriteStartElement("SINGLESKIMMATRIXPARA");
                    writer.WriteAttributeString("CALCULATE", "1");
                    writer.WriteAttributeString("Name", type.GetCalculationName());
                    writer.WriteAttributeString("SAVETOFILE", "0");
                    // end SINGLESKIMMATRIXPARA
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                // end OPERATION
                writer.WriteEndElement();
            });
            // Wipe out the previous procedures and run this.
            _visum.Procedures.OpenXmlWithOptions(tempFileName, ResetFunctionsBeforeReading: false);
            _visum.Procedures.Execute();
            var ret = new List<VisumMatrix>();
            foreach (var type in types)
            {
                ret.Add(GetMatrixByNameInner(type.GetMatrixName(segment)));
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            Files.SafeDelete(tempFileName);
            _lock.ExitWriteLock();
        }
    }

    private void ClearMatrices(VisumDemandSegment segment, List<PrTLosTypes> types)
    {
        foreach (var type in types)
        {
            // We can ignore the result because it is fine if nothing was removed.
            _ = DeleteMatrixByNameInner(type.GetMatrixName(segment));
        }
    }

    /// <summary>
    /// Calculate a Road LoS attribute and generate a matrix.
    /// </summary>
    /// <param name="demandSegment">The demand segment to process.</param>
    /// <param name="type">The PrT Skim matrix to generate.</param>
    /// <exception cref="VisumException">Can throw an exception if VISUM has an issue when processing the command.</exception>
    public VisumMatrix CalculateRoadLoS(VisumDemandSegment demandSegment, PrTLosTypes type)
    {
        return CalculateRoadLoS(demandSegment, [type])[0];
    }

}

