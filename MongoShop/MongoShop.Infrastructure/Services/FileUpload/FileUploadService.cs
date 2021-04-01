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

        /// <summary>
        /// Upload images to wwwroot/uploads folder with new random file name.
        /// </summary>
        /// <param name="imagesUpload">List of file to upload.</param>
        /// <returns>List of file paths.</returns>
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
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", Path.GetRandomFileName());

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
