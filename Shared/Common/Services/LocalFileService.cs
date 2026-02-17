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
                string path = "";
                if (file.Length > 0)
                {
                    path = Path.GetFullPath("C:\\Users\\Jared\\Desktop\\setela_v3\\UploadArea\\" + file.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                return Result<string>.Ok(path);
            } catch (Exception e)
            {
                return Result<string>.Fail(e.Message);
            }
        }
    }
}
