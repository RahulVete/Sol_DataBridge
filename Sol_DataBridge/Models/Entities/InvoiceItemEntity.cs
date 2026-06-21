namespace Sol_DataBridge.Models.Entities;

public class InvoiceItemEntity
{
    public Guid InvoiceItemID { get; set; }

    public Guid InvoiceID { get; set; }

    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    public string Unit { get; set; }

    public decimal Quantity { get; set; }

    public decimal Rate { get; set; }

    public decimal MRP { get; set; }

    public decimal Amount { get; set; }

    public decimal IGST { get; set; }

    public decimal TotalAmount { get; set; }
}