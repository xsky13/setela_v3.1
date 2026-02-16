using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Shared.Common.Services
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<Result<object>> UploadFile(List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var path = Path.GetFullPath("C:\\Users\\Jared\\Desktop\\setela_v3\\UploadArea\\" + file.FileName);
                        using (var stream = System.IO.File.Create(path))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
                return Result<object>.Ok(new { Success = true });
            } catch(Exception e)
            {
                return Result<object>.Fail(e.Message);
            }
        }
    }
}
