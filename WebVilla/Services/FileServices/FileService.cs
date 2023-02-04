namespace WebVilla.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHost;
        public FileService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public string AddFile(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public string UpdateFile(IFormFile file, string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
