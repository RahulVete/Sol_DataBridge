namespace Sol_DataBridge.Models.DTOs;

public class AssortmentDto
{
    public string itemcode { get; set; }

    public string uid { get; set; }

    public List<PackingInfoDto> packingInfo { get; set; }
}