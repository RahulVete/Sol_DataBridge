using Microsoft.Data.SqlClient;
using System.Data;
using Sol_DataBridge.Services.Interfaces;

namespace Sol_DataBridge.Services;

public class BulkCopyService : IBulkCopyService
{
    public async Task BulkInsertAsync(
        DataTable table,
        string destinationTable,
        SqlConnection connection,
        SqlTransaction transaction)
    {
        using SqlBulkCopy bulkCopy =
            new SqlBulkCopy(
                connection,
                SqlBulkCopyOptions.TableLock,
                transaction);

        bulkCopy.DestinationTableName =
            destinationTable;

        bulkCopy.BatchSize = 10000;

        bulkCopy.BulkCopyTimeout = 0;

        bulkCopy.EnableStreaming = true;

        foreach (DataColumn column in table.Columns)
        {
            bulkCopy.ColumnMappings.Add(
                column.ColumnName,
                column.ColumnName);
        }

        await bulkCopy.WriteToServerAsync(table);
    }
}