using Sol_DataBridge.Services.Interfaces;

namespace Sol_DataBridge.Services;

public class FileService : IFileService
{
    private readonly IConfiguration _configuration;

    public FileService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        string folder =
            _configuration["ImportSettings:TempFolder"];

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string filePath =
            Path.Combine(folder,
            $"{Guid.NewGuid()}_{file.FileName}");

        using FileStream stream =
            new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);

        return filePath;
    }

    public void DeleteFile(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}