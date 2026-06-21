namespace Sol_DataBridge.Models.DTOs;

public class ItemDto
{
    public string itemcode { get; set; }

    public string itemname { get; set; }

    public string unit { get; set; }

    public decimal quantity { get; set; }

    public decimal rate { get; set; }

    public decimal mrp { get; set; }

    public decimal amount { get; set; }

    public decimal igst { get; set; }

    public decimal totalamount { get; set; }
}