namespace WebVilla.Services.FileServices
{
    public interface IFileService
    {
        string AddFile(IFormFile file);
        bool DeleteFile(string filePath);
        string UpdateFile(IFormFile file, string filePath);
    }
}
