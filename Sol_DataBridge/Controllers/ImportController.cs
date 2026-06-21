using Microsoft.AspNetCore.Mvc;
using Sol_DataBridge.Services.Interfaces;

namespace Sol_DataBridge.Controllers;

[ApiController]
[Route("api/import")]
public class ImportController : ControllerBase
{
    private readonly IImportService _importService;

    public ImportController(
        IImportService importService)
    {
        _importService = importService;
    }

    [HttpPost("invoice")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult>
        UploadInvoice(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File missing");

        await _importService.ImportAsync(file);

        return Ok(new
        {
            Message = "Import Started"
        });
    }
}