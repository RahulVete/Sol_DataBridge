namespace Sol_DataBridge.Models.Entities;

public class PackingInfoEntity
{
    public Guid PackingInfoID { get; set; }

    public Guid AssortmentID { get; set; }

    public string ItemCode { get; set; }

    public decimal Quantity { get; set; }
}