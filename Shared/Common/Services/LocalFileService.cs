using MediatR;
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

        public async Task<Result<bool>> VerifyMultipleForSubmission(List<IFormFile> files)
        {
            if (files.Count > 7) return Result<bool>.Fail("Maximo de 7 archivos.");

            foreach (var file in files)
            {
                if (file.Length > 10 * 1024 * 1024)
                    return Result<bool>.Fail($"El archivo {file.FileName} excede el límite de 10MB.");

                var ext = Path.GetExtension(file.FileName).ToLower();
                var allowed = new[] { ".pdf", ".zip", ".docx", ".doc", ".jpg", ".png" };
                if (!allowed.Contains(ext))
                    return Result<bool>.Fail($"El tipo de archivo {ext} no está permitido.");
            }
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> VerifyMultiple(List<IFormFile> files)
        {
            if (files.Count > 7) return Result<bool>.Fail("Maximo de 7 archivos.");

            foreach (var file in files)
            {
                if (file.Length > 10 * 1024 * 1024)
                    return Result<bool>.Fail($"El archivo {file.FileName} excede el límite de 10MB.");

                var ext = Path.GetExtension(file.FileName).ToLower();
                var allowed = new[]
                { 
                    // Documents
                    ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".rtf", ".odt",
                    // Images
                    ".jpg", ".jpeg", ".png", ".gif", ".svg", ".webp",
                    // Audio/Video
                    ".mp3", ".wav", ".mp4", ".mov", ".m4a",
                    // Archives (for code or large datasets)
                    ".zip", ".rar", ".7z", ".tar.gz"
                };
                if (!allowed.Contains(ext))
                    return Result<bool>.Fail($"El tipo de archivo {ext} no está permitido.");
            }
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> VerifySingle(IFormFile file)
        {

            if (file.Length > 10 * 1024 * 1024)
                return Result<bool>.Fail($"El archivo {file.FileName} excede el límite de 10MB.");

            var ext = Path.GetExtension(file.FileName).ToLower();

            var allowed = new[]
            { 
                // Documents
                ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".rtf", ".odt",
                // Images
                ".jpg", ".jpeg", ".png", ".gif", ".svg", ".webp",
                // Audio/Video
                ".mp3", ".wav", ".mp4", ".mov", ".m4a",
                // Archives (for code or large datasets)
                ".zip", ".rar", ".7z", ".tar.gz"
            };

            if (!allowed.Contains(ext))
                return Result<bool>.Fail($"El tipo de archivo {ext} no está permitido.");

            return Result<bool>.Ok(true);
        }
    }
}
