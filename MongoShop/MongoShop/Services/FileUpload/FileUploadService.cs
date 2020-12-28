
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace MongoShop.Services.FileUpload
{
    public class FileUploadService : IFileUploadService
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<string>> Upload(ImagesUpload imagesUpload)
        {
            var filePaths = new List<string>();
            foreach (var formFile in imagesUpload.Files)
            {
                if (formFile.Length > 0)
                {

                    // get the file path with new random file name.
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", Path.GetRandomFileName());

                    path = path.Replace(Path.GetExtension(path), Path.GetExtension(formFile.FileName));

                    filePaths.Add(path);

                    using (var stream = File.Create(path))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return filePaths;
        }
    }
}
