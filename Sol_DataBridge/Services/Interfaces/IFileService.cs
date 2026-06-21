namespace Sol_DataBridge.Services.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);

    void DeleteFile(string path);
}