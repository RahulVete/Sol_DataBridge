namespace Sol_DataBridge.Services.Interfaces;

public interface IImportService
{
    Task ImportAsync(IFormFile file);
}