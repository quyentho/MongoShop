using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MongoShop.Services.FileUpload
{
    public interface IFileUploadService
    {
        Task<List<string>> Upload(List<IFormFile> imagesUpload);
    }
}
