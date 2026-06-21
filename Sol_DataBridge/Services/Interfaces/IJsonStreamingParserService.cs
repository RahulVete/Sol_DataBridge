using Sol_DataBridge.Models.DTOs;

namespace Sol_DataBridge.Services.Interfaces;

public interface IJsonStreamingParserService
{
    Task<InvoiceDto> ParseInvoiceAsync(string filePath);
}