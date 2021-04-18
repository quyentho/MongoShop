using AutoMapper;
using MongoShop.SharedModels.Category;
using MongoShop.SharedModels.Order;
using MongoShop.SharedModels.Product;

namespace MongoShop.UI
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ProductViewModel, EditProductRequest>();
        }
    }
}