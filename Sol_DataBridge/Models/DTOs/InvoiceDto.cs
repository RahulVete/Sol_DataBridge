namespace Sol_DataBridge.Models.DTOs;

public class InvoiceDto
{
    public string invoiceno { get; set; }

    public DateTime invoicedate { get; set; }

    public string partycode { get; set; }

    public string partyname { get; set; }

    public string masterid { get; set; }

    public decimal grossamount { get; set; }

    public string sono { get; set; }

    public string sodate { get; set; }

    public string pono { get; set; }

    public string podate { get; set; }

    public List<ItemDto> itemlist { get; set; }

    public List<AssortmentDto> assortmentdetail { get; set; }
}