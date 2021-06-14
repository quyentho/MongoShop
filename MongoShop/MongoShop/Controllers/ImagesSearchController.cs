using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoShop.Controllers
{
    public class ImagesSearchController : Controller
    {
        public IActionResult Upload(ImageUploadModel)
        {

            return View();
        }
    }

    public class ImageUploadModel
    {
        public List<IFormFile> ImagesUpload { get; set; }
    }
}
