using Dapper;
using Microsoft.Data.SqlClient;
using Sol_DataBridge.Models.Entities;
using Sol_DataBridge.Repositories.Interfaces;
using System.Data;

namespace Sol_DataBridge.Repositories;

public class ImportRepository : IImportRepository
{
    public async Task TruncateStagingTablesAsync(
        SqlConnection connection,
        SqlTransaction transaction)
    {
        await connection.ExecuteAsync(
            "dbo.usp_TruncateStagingTables",
            transaction: transaction,
            commandType: CommandType.StoredProcedure);
    }

    public async Task ExecuteMoveProcedureAsync(
        Guid importBatchId,
        SqlConnection connection,
        SqlTransaction transaction)
    {
        await connection.ExecuteAsync(
            "dbo.usp_MoveStagingToFinal",
            new
            {
                ImportBatchID = importBatchId
            },
            transaction: transaction,
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateImportStatusAsync(
        Guid batchId,
        string status,
        SqlConnection connection,
        SqlTransaction transaction)
    {
        await connection.ExecuteAsync(
            "dbo.usp_ImportBatch_UpdateStatusTime",
            new
            {
                ImportBatchID = batchId,
                Status = status
            },
            transaction: transaction,
            commandType: CommandType.StoredProcedure);
    }

    public async Task SaveValidationErrorsAsync(
        List<ValidationErrorEntity> errors,
        SqlConnection connection,
        SqlTransaction transaction)
    {
        await connection.ExecuteAsync(
            "dbo.usp_ValidationError_Insert",
            errors,
            transaction: transaction,
            commandType: CommandType.StoredProcedure);
    }
}