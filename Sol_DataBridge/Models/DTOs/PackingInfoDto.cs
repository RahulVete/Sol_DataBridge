namespace Sol_DataBridge.Models.DTOs;

public class PackingInfoDto
{
    public string itemcode { get; set; }

    public decimal quantity { get; set; }

    public List<PairDetailDto> pairdetail { get; set; }
}