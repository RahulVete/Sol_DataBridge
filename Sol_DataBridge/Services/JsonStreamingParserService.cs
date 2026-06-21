using System.Text.Json;
using Sol_DataBridge.Models.DTOs;
using Sol_DataBridge.Services.Interfaces;

namespace Sol_DataBridge.Services;

public class JsonStreamingParserService :
    IJsonStreamingParserService
{
    public async Task<InvoiceDto>
        ParseInvoiceAsync(string filePath)
    {
        await using FileStream fs =
            File.OpenRead(filePath);

        return await JsonSerializer.DeserializeAsync<InvoiceDto>(
            fs,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }


}