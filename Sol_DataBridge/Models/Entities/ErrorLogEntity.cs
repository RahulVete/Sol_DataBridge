namespace Sol_DataBridge.Models.Entities;

public class ErrorLogEntity
{
    public Guid ImportBatchID { get; set; }

    public string SourceClass { get; set; }

    public string SourceMethod { get; set; }

    public string ErrorMessage { get; set; }

    public string? InnerException { get; set; }

    public string? StackTrace { get; set; }

    public string? JsonSnippet { get; set; }
}