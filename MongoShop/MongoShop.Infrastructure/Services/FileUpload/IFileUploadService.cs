using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MongoShop.Infrastructure.Services.FileUpload
{
    public interface IFileUploadService
    {
        Task<List<string>> Upload(List<IFormFile> imagesUpload);
    }
}
