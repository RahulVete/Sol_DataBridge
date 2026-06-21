using Microsoft.Data.SqlClient;
using Sol_DataBridge.Models.Entities;

public interface IImportStatusService
{
    Task CreateBatchAsync(
        ImportBatchEntity batch,
        SqlConnection connection,
        SqlTransaction transaction);

    Task UpdateStatusAsync(
        Guid batchId,
        string status,
        string currentStep,
        SqlConnection connection,
        SqlTransaction transaction);
}