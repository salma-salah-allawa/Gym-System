using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Contracts
{
    public interface IAttatchmentService
    {
        Task<string?> UploadAsync(Stream stream, string fileName, string folderName, CancellationToken ct);
        bool DeleteAsync(string fileName,string folderName);

        (Stream stream,string contentType)? GetAsync(string fileName,string folderName);
    }
}
