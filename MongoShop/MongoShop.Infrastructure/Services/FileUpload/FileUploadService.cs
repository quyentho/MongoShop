using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MongoShop.Infrastructure.Services.FileUpload
{
    public class FileUploadService : IFileUploadService
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        /// 
        public async Task<List<string>> Upload(List<IFormFile> imagesUpload)
        {
            if (imagesUpload is null)
            {
                return null;
            }

            var filePaths = new List<string>();
            foreach (var file in imagesUpload)
            {
                if (file.Length > 0)
                {
                    // get the file path with new random file name.
                    string path;
                    if (_webHostEnvironment.WebRootPath != null) //this is web mvc project.
                    {
                        
                        var folderPath = Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, "uploads"));
                        path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", Path.GetRandomFileName());
                    }
                    else //this is web api project.
                    {
                        var folderPath = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "uploads"));
                        path = Path.Combine(folderPath.FullName, Path.GetRandomFileName());
                    }

                    path = path.Replace(Path.GetExtension(path), Path.GetExtension(file.FileName));

                    using (var stream = File.Create(path))
                    {
                        await file.CopyToAsync(stream);
                    }

                    path = Path.GetFileName(path);
                    path = "/uploads/" + path;
                    filePaths.Add(path);
                }
            }

            return filePaths;
        }
    }
}
