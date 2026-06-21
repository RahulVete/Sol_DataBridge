using Microsoft.Data.SqlClient;
using System.Data;

namespace Sol_DataBridge.Services.Interfaces;

public interface IBulkCopyService
{
    Task BulkInsertAsync(
        DataTable table,
        string destinationTable,
        SqlConnection connection,
        SqlTransaction transaction);
}