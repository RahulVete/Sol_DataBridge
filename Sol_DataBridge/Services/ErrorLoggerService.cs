using Dapper;
using Microsoft.Data.SqlClient;
using Sol_DataBridge.Models.Entities;
using System.Data;

namespace Sol_DataBridge.Services;

public class ErrorLoggerService : IErrorLoggerService
{
    public async Task LogErrorAsync(
        ErrorLogEntity error,
        SqlConnection connection,
        SqlTransaction transaction)
    {
        await connection.ExecuteAsync(
            "dbo.usp_ErrorLogger_Insert",
            error,
            transaction: transaction,
            commandType: CommandType.StoredProcedure);
    }
}