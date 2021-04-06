using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MongoShop.Infrastructure.Services.FileUpload
{
    public interface IFileUploadService
    {
        /// <summary>
        /// Upload images to wwwroot/uploads folder with new random file name.
        /// </summary>
        /// <param name="imagesUpload">List of file to upload.</param>
        /// <returns>List of uploaded file paths. Null if no image to upload.</returns>
        Task<List<string>> Upload(List<IFormFile> imagesUpload);
    }
}
