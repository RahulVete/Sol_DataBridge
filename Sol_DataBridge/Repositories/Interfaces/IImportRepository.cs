using Microsoft.Data.SqlClient;
using Sol_DataBridge.Models.Entities;
using System.Data;

namespace Sol_DataBridge.Repositories.Interfaces;

public interface IImportRepository
{
    Task TruncateStagingTablesAsync(
        SqlConnection connection,
        SqlTransaction transaction);

    Task ExecuteMoveProcedureAsync(
        Guid importBatchId,
        SqlConnection connection,
        SqlTransaction transaction);

    Task UpdateImportStatusAsync(
        Guid batchId,
        string status,
        SqlConnection connection,
        SqlTransaction transaction);

    Task SaveValidationErrorsAsync(
List<ValidationErrorEntity> errors,
SqlConnection connection,
SqlTransaction transaction);

}