using MongoShop.BusinessDomain.Categories;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.ImagesSearch
{
    public interface IImagesSearchService
    {
        Task<UploadedImage> AddAsync(UploadedImage image);
    }
}
