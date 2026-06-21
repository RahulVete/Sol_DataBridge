namespace Sol_DataBridge.Models.Entities;

public class AssortmentEntity
{
    public Guid AssortmentID { get; set; }

    public Guid InvoiceID { get; set; }

    public string ItemCode { get; set; }

    public string UID { get; set; }
}