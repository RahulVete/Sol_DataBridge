using Microsoft.Data.SqlClient;
using Sol_DataBridge.Models.Entities;

public interface IErrorLoggerService
{
    Task LogErrorAsync(
        ErrorLogEntity error,
        SqlConnection connection,
        SqlTransaction transaction);
}