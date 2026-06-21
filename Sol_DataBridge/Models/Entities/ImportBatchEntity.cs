namespace Sol_DataBridge.Models.Entities;

public class ImportBatchEntity
{
    public Guid ImportBatchID { get; set; }

    public string FileName { get; set; }

    public string Status { get; set; }

    public string CurrentStep { get; set; }

    public long TotalRecords { get; set; }

    public long SuccessRecords { get; set; }

    public long FailedRecords { get; set; }
}