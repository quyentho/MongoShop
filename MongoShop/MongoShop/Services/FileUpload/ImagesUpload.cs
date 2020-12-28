using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace MongoShop.Services.FileUpload
{
    public class ImagesUpload
    {
        public List<IFormFile> Files { get; set; }
    }
}
