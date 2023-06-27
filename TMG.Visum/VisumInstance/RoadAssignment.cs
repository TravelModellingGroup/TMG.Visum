using System.Xml;

namespace TMG.Visum;

public partial class VisumInstance
{
    public void ExecuteRoadAssignment(VisumDemandSegment segment)
    {
        ExecuteRoadAssignment(new List<VisumDemandSegment>(1) { segment });
    }

    public void ExecuteRoadAssignment(IEnumerable<VisumDemandSegment> demandSegments)
    {
        _lock.EnterWriteLock();
        string? tempFileName = null;
        try
        {
            ObjectDisposedException.ThrowIf(_visum is null, this);
            tempFileName = WriteProcedure((writer) =>
            {
                writer.WriteStartElement("OPERATION");
                writer.WriteAttributeString("ACTIVE", "1");
                writer.WriteAttributeString("NO", "1");
                writer.WriteAttributeString("OPERATIONTYPE", "PrTAssignment");
                writer.WriteAttributeString("PARENTGROUPINDEX", "0");

                writer.WriteStartElement("PRTASSIGNMENTPARA");
                writer.WriteAttributeString("DSEGSET", string.Join(',', demandSegments.Select(seg => seg.Code)));
                writer.WriteAttributeString("PRTASSIGNMENTVARIANT", "Equilibrium");

                writer.WriteStartElement("EQUILIBRIUMPARA");
                writer.WriteAttributeString("MAXITERATIONS", "100");
                writer.WriteAttributeString("USECURRENTSOLUTION", "0");
                writer.WriteAttributeString("USEEXTENDEDSTABILITYCRITERIA", "0");

                writer.WriteStartElement("EXTENDEDSTABILITYCRITERIAPARA");
                writer.WriteAttributeString("FRACTIONMAXRELDIFFLINKIMP", "0.98");
                writer.WriteAttributeString("FRACTIONMAXRELDIFFLINKVOL", "0.98");
                writer.WriteAttributeString("FRACTIONMAXRELDIFFTURNIMP", "0.98");
                writer.WriteAttributeString("FRACTIONMAXRELDIFFTURNVOL", "0.98");
                writer.WriteAttributeString("IGNOREVOLUMESMALLERTHAN", "0");
                writer.WriteAttributeString("MAXGAP", "0.0001");
                writer.WriteAttributeString("MAXRELDIFFLINKIMP", "0.01");
                writer.WriteAttributeString("MAXRELDIFFLINKVOL", "0.01");
                writer.WriteAttributeString("MAXRELDIFFTURNIMP", "0.01");
                writer.WriteAttributeString("MAXRELDIFFTURNVOL", "0.01");
                writer.WriteAttributeString("NUMCHECKEDSUBSEQUENTITERATIONS", "4");
                writer.WriteAttributeString("ONLYACTIVENETOBJECTS", "0");
                writer.WriteEndElement();

                // end EQUILIBRIUMPARA
                writer.WriteEndElement();
                // end PRTASSIGNMENTPARA
                writer.WriteEndElement();
                // end OPERATION
                writer.WriteEndElement();
            });
            // Wipe out the previous procedures and run this.
            _visum.Procedures.OpenXml(tempFileName, false);
            _visum.Procedures.Execute();
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
}
