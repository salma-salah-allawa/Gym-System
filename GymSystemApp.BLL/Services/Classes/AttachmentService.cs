using GymSystemApp.BLL.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GymSystemApp.BLL.Services.Classes
{
    public class AttachmentService : IAttatchmentService
    {
        private readonly long maxFileSize = 5* 1024 * 1024;
        private readonly string[] allowedExtentions = { ".jpg", ".png", ".jpeg" };
        private readonly ILogger logger;
        private readonly IWebHostEnvironment env;

        public AttachmentService(ILogger<AttachmentService> logger,IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }
        public bool DeleteAsync(string fileName, string folderName)
        {
           if(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;
            try
            {
                var fullPath= Path.Combine(env.ContentRootPath,folderName, fileName);
                if(!File.Exists(fullPath)) return false;
                File.Delete(fullPath);
                return true;
                
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Filed to delete the file.");
                return false;
            }
        }

        public (Stream stream, string contentType)? GetAsync(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName) ||string.IsNullOrWhiteSpace(folderName)) return null;

            try
            {
                var fullPath = Path.Combine(env.ContentRootPath, folderName, fileName);

                if (!File.Exists(fullPath))
                    return null;

                var stream = new FileStream(fullPath,FileMode.Open,FileAccess.Read,FileShare.Read);
                var extension = Path.GetExtension(fileName).ToLower();
                var contentType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    _ => "application/octet-stream"
                };
                return (stream, contentType);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed To Get File.");
                return null;
            }
        }

        public async Task<string?> UploadAsync(Stream stream, string fileName, string folderName, CancellationToken ct)
        {
            if (stream is null || !stream.CanRead) return null;

            if (stream.Length == 0) return null;
            if(stream.Length > maxFileSize)
            {
                logger.LogWarning("Rejected Because File Is Too Large.");
                return null;
            }

            var extention =Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extention) || !allowedExtentions.Contains(extention))
            {
                logger.LogWarning("Rejected Because File Has Wrong Extention.");
                return null;
            }

            var UploadedFolder = Path.Combine(env.ContentRootPath, folderName);
            Directory.CreateDirectory(UploadedFolder);
            var storedFileName= $"{Guid.NewGuid()}{extention}";
            var filePath= Path.Combine(UploadedFolder, storedFileName);

            try
            {
                await using var fs= new FileStream(filePath,FileMode.CreateNew,FileAccess.Write,FileShare.None);
                await stream.CopyToAsync(fs);
                return storedFileName;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falied To Uplode File.");
                return null;
            }
        }
    }
}
