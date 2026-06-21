namespace Sol_DataBridge.Models.Entities;

public class ValidationErrorEntity
{
    public Guid ImportBatchID { get; set; }

    public string EntityName { get; set; }

    public string FailedData { get; set; }

    public string ValidationMessage { get; set; }
}