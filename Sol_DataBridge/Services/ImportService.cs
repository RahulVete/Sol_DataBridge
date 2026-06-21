using Microsoft.Data.SqlClient;
using Sol_DataBridge.Data;
using Sol_DataBridge.Helpers;
using Sol_DataBridge.Models.DTOs;
using Sol_DataBridge.Models.Entities;
using Sol_DataBridge.Repositories.Interfaces;
using Sol_DataBridge.Services.Interfaces;
using System.Data;

namespace Sol_DataBridge.Services;

public class ImportService : IImportService
{
    private readonly IFileService _fileService;
    private readonly IJsonStreamingParserService _parser;
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly IImportRepository _repository;
    private readonly IBulkCopyService _bulkCopy;
    private readonly IValidationService _validationService;
    private readonly IErrorLoggerService _errorLogger;
    private readonly IImportStatusService _statusService;

    public ImportService(
        IFileService fileService,
        IJsonStreamingParserService parser,
        ISqlConnectionFactory connectionFactory,
        IImportRepository repository,
        IBulkCopyService bulkCopy,
        IValidationService validationService,
        IErrorLoggerService errorLogger,
        IImportStatusService statusService)
    {
        _fileService = fileService;
        _parser = parser;
        _connectionFactory = connectionFactory;
        _repository = repository;
        _bulkCopy = bulkCopy;
        _validationService = validationService;
        _errorLogger = errorLogger;
        _statusService = statusService;
    }

    public async Task ImportAsync(IFormFile file)
    {
        Guid importBatchId = Guid.NewGuid();

        string filePath = string.Empty;

        SqlConnection? connection = null;
        SqlTransaction? transaction = null;

        try
        {
            #region Save File

            filePath = await _fileService.SaveFileAsync(file);

            #endregion

            #region Parse JSON

            InvoiceDto invoice =
                await _parser.ParseInvoiceAsync(filePath);

            if (invoice == null)
            {
                throw new Exception("Invalid JSON File.");
            }

            #endregion

            #region Generate Entity Hierarchy

            Guid invoiceId = Guid.NewGuid();

            InvoiceEntity invoiceEntity =
                new InvoiceEntity
                {
                    InvoiceID = invoiceId,
                    ImportBatchID = importBatchId,
                    InvoiceNo = invoice.invoiceno,
                    InvoiceDate = invoice.invoicedate,
                    PartyCode = invoice.partycode,
                    PartyName = invoice.partyname,
                    MasterID = invoice.masterid,
                    GrossAmount = invoice.grossamount,
                    SoNo = invoice.sono,
                    SoDate = DateTime.TryParse(invoice.sodate?.ToString(), out var dtso)? dtso: null,
                    PoNo = invoice.pono,
                    PoDate = DateTime.TryParse(invoice.podate?.ToString(), out var dtpo) ? dtpo : null
                };

            List<InvoiceItemEntity> itemEntities = new();

            foreach (var item in invoice.itemlist ?? Enumerable.Empty<ItemDto>())
            {
                itemEntities.Add(
                    new InvoiceItemEntity
                    {
                        InvoiceItemID = Guid.NewGuid(),
                        InvoiceID = invoiceId,
                        ItemCode = item.itemcode,
                        ItemName = item.itemname,
                        Unit = item.unit,
                        Quantity = item.quantity,
                        Rate = item.rate,
                        MRP = item.mrp,
                        Amount = item.amount,
                        IGST = item.igst,
                        TotalAmount = item.totalamount
                    });
            }

            List<AssortmentEntity> assortmentEntities = new();

            List<PackingInfoEntity> packingEntities = new();

            List<PairDetailEntity> pairEntities = new();

            foreach (var assortment
                     in invoice.assortmentdetail ?? Enumerable.Empty<AssortmentDto>())
            {
                Guid assortmentId = Guid.NewGuid();

                assortmentEntities.Add(
                    new AssortmentEntity
                    {
                        AssortmentID = assortmentId,
                        InvoiceID = invoiceId,
                        ItemCode = assortment.itemcode,
                        UID = assortment.uid
                    });

                foreach (var packing
                         in assortment.packingInfo ?? Enumerable.Empty<PackingInfoDto>())
                {
                    Guid packingId = Guid.NewGuid();

                    packingEntities.Add(
                        new PackingInfoEntity
                        {
                            PackingInfoID = packingId,
                            AssortmentID = assortmentId,
                            ItemCode = packing.itemcode,
                            Quantity = packing.quantity
                        });

                    foreach (var pair
                             in packing.pairdetail ?? Enumerable.Empty<PairDetailDto>())
                    {
                        pairEntities.Add(
                            new PairDetailEntity
                            {
                                PairDetailID = Guid.NewGuid(),
                                PackingInfoID = packingId,
                                PairQR = pair.pairqr,
                                PairUID = pair.pairuid
                            });
                    }
                }
            }

            #endregion

            #region Open SQL Connection

            connection = _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            transaction =
                (SqlTransaction)await connection.BeginTransactionAsync();

            #endregion

            #region Create Import Batch

            await _statusService.CreateBatchAsync(
                new ImportBatchEntity
                {
                    ImportBatchID = importBatchId,
                    FileName = file.FileName,
                    Status = "Started",
                    CurrentStep = "Parsing",
                    TotalRecords = 1,
                    SuccessRecords = 0,
                    FailedRecords = 0
                },
                connection,
                transaction);

            #endregion

            #region Validation

            await _statusService.UpdateStatusAsync(
                importBatchId,
                "Running",
                "Validation",
                connection,
                transaction);

            var validationErrors =
                _validationService.ValidateInvoices(
                    new List<InvoiceEntity>
                    {
                        invoiceEntity
                    });

            if (validationErrors.Any())
            {
                await _repository.SaveValidationErrorsAsync(
                    validationErrors,
                    connection,
                    transaction);

                throw new Exception(
                    "Validation failed. Check tblValidationError.");
            }

            #endregion

            #region Truncate Staging

            await _statusService.UpdateStatusAsync(
                importBatchId,
                "Running",
                "TruncateStaging",
                connection,
                transaction);

            await _repository.TruncateStagingTablesAsync(
                connection,
                transaction);

            #endregion

            #region Bulk Copy

            await _statusService.UpdateStatusAsync(
                importBatchId,
                "Running",
                "BulkCopy",
                connection,
                transaction);

            DataTable invoiceTable =
                DataTableFactory.CreateInvoiceTable(
                    new List<InvoiceEntity>
                    {
                        invoiceEntity
                    });

            if (invoiceTable.Rows.Count > 0)
            {
                await _bulkCopy.BulkInsertAsync(
                    invoiceTable,
                    "tbl_Invoice_Stg",
                    connection,
                    transaction);
            }

            DataTable itemTable =
                DataTableFactory.CreateItemTable(itemEntities);

            if (itemTable.Rows.Count > 0)
            {
                await _bulkCopy.BulkInsertAsync(
                    itemTable,
                    "tbl_ItemList_Stg",
                    connection,
                    transaction);
            }

            DataTable assortmentTable =
                DataTableFactory.CreateAssortmentTable(
                    assortmentEntities);

            if (assortmentTable.Rows.Count > 0)
            {
                await _bulkCopy.BulkInsertAsync(
                    assortmentTable,
                    "tbl_AssortmentDetail_Stg",
                    connection,
                    transaction);
            }

            DataTable packingTable =
                DataTableFactory.CreatePackingTable(
                    packingEntities);

            if (packingTable.Rows.Count > 0)
            {
                await _bulkCopy.BulkInsertAsync(
                    packingTable,
                    "tbl_PackingInfo_Stg",
                    connection,
                    transaction);
            }

            DataTable pairTable =
                DataTableFactory.CreatePairTable(
                    pairEntities);

            if (pairTable.Rows.Count > 0)
            {
                await _bulkCopy.BulkInsertAsync(
                    pairTable,
                    "tbl_PairDetail_Stg",
                    connection,
                    transaction);
            }

            #endregion

            #region Move Staging To Final

            await _statusService.UpdateStatusAsync(
                importBatchId,
                "Running",
                "MoveToFinal",
                connection,
                transaction);

            await _repository.ExecuteMoveProcedureAsync(
                importBatchId,
                connection,
                transaction);

            #endregion

            #region Delete Temp File

            if (!string.IsNullOrWhiteSpace(filePath) &&
                File.Exists(filePath))
            {
                _fileService.DeleteFile(filePath);
            }

            #endregion

            #region Complete Import

            await _statusService.UpdateStatusAsync(
                importBatchId,
                "Completed",
                "Completed",
                connection,
                transaction);

            await transaction.CommitAsync();

            #endregion
        }
        catch (Exception ex)
        {
            try
            {
                if (connection != null &&
                    transaction != null)
                {
                    await _errorLogger.LogErrorAsync(
                        new ErrorLogEntity
                        {
                            ImportBatchID = importBatchId,
                            SourceClass = nameof(ImportService),
                            SourceMethod = nameof(ImportAsync),
                            ErrorMessage = ex.Message,
                            InnerException =
                                ex.InnerException?.Message ?? string.Empty,
                            StackTrace =
                                ex.StackTrace ?? string.Empty,
                            JsonSnippet =
                                file.FileName
                        },
                        connection,
                        transaction);

                    await _statusService.UpdateStatusAsync(
                        importBatchId,
                        "Failed",
                        "Error",
                        connection,
                        transaction);
                }
            }
            catch
            {
                // Ignore logging failure
            }

            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }

            throw;
        }
        finally
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
            }

            if (connection != null)
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }
    }
}