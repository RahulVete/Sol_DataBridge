using Dapper;
using Microsoft.Data.SqlClient;
using Sol_DataBridge.Models.Entities;
using System.Data;

namespace Sol_DataBridge.Services;

public class ImportStatusService : IImportStatusService
{
    public async Task CreateBatchAsync(
        ImportBatchEntity batch,
        SqlConnection connection,
        SqlTransaction transaction)
    {
        await connection.ExecuteAsync(
            "dbo.usp_ImportBatch_Create",
            batch,
            transaction: transaction,
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateStatusAsync(
        Guid batchId,
        string status,
        string currentStep,
        SqlConnection connection,
        SqlTransaction transaction)
    {
        await connection.ExecuteAsync(
            "dbo.usp_ImportBatch_UpdateStatus",
            new
            {
                ImportBatchID = batchId,
                Status = status,
                CurrentStep = currentStep
            },
            transaction: transaction,
            commandType: CommandType.StoredProcedure);
    }
}