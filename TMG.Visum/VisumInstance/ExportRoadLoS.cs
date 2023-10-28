// Ignore Spelling: Visum
using TMG.Visum.RoadAssignment;
namespace TMG.Visum;

public partial class VisumInstance
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="demandSegment"></param>
    /// <exception cref="VisumException"></exception>
    public VisumMatrix CalculateRoadLoS(VisumDemandSegment demandSegment, PrTLosTypes type)
    {
        string? tempFileName = null;
        try
        {
            _lock.EnterWriteLock();
            ObjectDisposedException.ThrowIf(_visum is null, this);
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
                writer.WriteAttributeString("DSEG", demandSegment.Code);
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
                writer.WriteStartElement("SINGLESKIMMATRIXPARA");
                writer.WriteAttributeString("CALCULATE", "1");
                writer.WriteAttributeString("Name", type.GetCalculationName());
                writer.WriteAttributeString("SAVETOFILE", "0");
                // end SINGLESKIMMATRIXPARA
                writer.WriteEndElement();

                // end PRTSKIMMATRIXPARA
                writer.WriteEndElement();
                // end OPERATION
                writer.WriteEndElement();

            });
            // Wipe out the previous procedures and run this.
            _visum.Procedures.OpenXmlWithOptions(tempFileName);
            _visum.Procedures.Execute();
            return GetMatrixByNameInner(type.GetMatrixName(demandSegment));
        }
        catch(Exception ex)
        {
            throw new VisumException(ex);
        }
        finally
        {
            Files.SafeDelete(tempFileName);
            _lock.ExitWriteLock();
        }
    }

}

