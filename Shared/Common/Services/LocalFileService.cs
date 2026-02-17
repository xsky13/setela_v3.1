using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Utilities;
using System.IO;

namespace SetelaServerV3._1.Shared.Common.Services
{
    public class LocalFileService : IFileStorage
    {
        public async Task<Result<string>> SaveFile(IFormFile file)
        {
            try
            {
                string fileName = "";
                if (file.Length > 0)
                {
                    fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var fullPath = Path.Combine("C:\\Users\\Jared\\Desktop\\setela_v3\\UploadArea\\", fileName);

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
