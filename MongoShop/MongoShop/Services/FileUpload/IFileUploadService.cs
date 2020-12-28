using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoShop.Services.FileUpload
{
    public interface IFileUploadService
    {
        Task<List<string>> Upload(ImagesUpload imagesUpload);
    }
}
