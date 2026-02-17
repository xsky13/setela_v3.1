using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Utilities;
using System.IO;

namespace SetelaServerV3._1.Shared.Common.Services
{
    public class LocalFileService : IFileStorage
    {
        private readonly string path = "C:\\Users\\Jared\\Desktop\\setela_v3\\UploadArea\\";

        public async Task<Result<object>> DeleteFile(string fileName, int userId)
        {
            var filePath = Path.Combine(path, $"{userId}/{Path.GetFileName(fileName)}");
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
            }
            else
            {
                return Result<object>.Fail("El archivo no existe");
            }
        }

        public async Task<Result<string>> SaveFile(IFormFile file, int userId)
        {
            try
            {
                string fileName = "";
                if (file.Length > 0)
                {

                    var userPath = Path.Combine(path, userId.ToString());

                    if (!Directory.Exists(userPath))
                        Directory.CreateDirectory(userPath);

                    fileName = Path.GetFileName(file.FileName);
                    string fullPath = Path.Combine(userPath, fileName);
                    if (File.Exists(fullPath))
                    {
                        fileName = $"{DateTime.Now.Ticks}_${file.FileName}";
                        fullPath = Path.Combine(userPath, fileName);
                    }


                    using (var stream = System.IO.File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                return Result<string>.Ok(fileName);
            }
            catch (Exception e)
            {
                return Result<string>.Fail(e.Message);
            }
        }
    }
}
