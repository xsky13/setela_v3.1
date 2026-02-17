using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Utilities;
using System.IO;

namespace SetelaServerV3._1.Shared.Common.Services
{
    public class LocalFileService : IFileStorage
    {
        private readonly string path = "C:\\Users\\Jared\\Desktop\\setela_v3\\UploadArea\\";

        public async Task<Result<object>> DeleteFile(string fileName)
        {
            var filePath = Path.Combine(path, fileName);
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                    return Result<object>.Ok(new { Success = true });
                }
                catch (IOException ex)
                {
                    return Result<object>.Fail(ex.Message);
                }
            } else
            {
                return Result<object>.Fail("El archivo no existe");
            }
        }

        public async Task<Result<string>> SaveFile(IFormFile file)
        {
            try
            {
                string fileName = "";
                if (file.Length > 0)
                {
                    fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var fullPath = Path.Combine(path, fileName);

                    using (var stream = System.IO.File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                return Result<string>.Ok(fileName);
            } catch (Exception e)
            {
                return Result<string>.Fail(e.Message);
            }
        }
    }
}
