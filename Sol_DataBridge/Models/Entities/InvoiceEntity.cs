namespace Sol_DataBridge.Models.Entities;

public class InvoiceEntity
{
    public Guid InvoiceID { get; set; }

    public Guid ImportBatchID { get; set; }

    public string InvoiceNo { get; set; }

    public DateTime InvoiceDate { get; set; }

    public string PartyCode { get; set; }

    public string PartyName { get; set; }

    public string MasterID { get; set; }

    public decimal GrossAmount { get; set; }

    public string SoNo { get; set; }

    public DateTime? SoDate { get; set; }

    public string PoNo { get; set; }

    public DateTime? PoDate { get; set; }
}