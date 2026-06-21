namespace Sol_DataBridge.Models.Entities;

public class PairDetailEntity
{
    public Guid PairDetailID { get; set; }

    public Guid PackingInfoID { get; set; }

    public string PairQR { get; set; }

    public string PairUID { get; set; }
}