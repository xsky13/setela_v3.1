using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Utilities;
using Supabase;
using Supabase.Storage;

namespace SetelaServerV3._1.Shared.Common.Services
{
    public class SupabaseFileService(Supabase.Client supabase) : IFileStorage
    {
        private readonly Supabase.Client _supabase = supabase;
        private const string BucketName = "uploads";

        public async Task<Result<object>> DeleteFile(string fileName, int userId)
        {
            try
            {
                string supabasePath = $"{userId}/{Path.GetFileName(fileName)}";

                // Delete requires a list of paths
                await _supabase.Storage
                    .From(BucketName)
                    .Remove(new List<string> { supabasePath });

                return Result<object>.Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                return Result<object>.Fail(ex.Message);
            }
        }

        private async Task<bool> IsFileExists(string path)
        {
            try
            {
                var res = _supabase.Storage.From(BucketName).GetPublicUrl(path);
                return false;
            }
            catch { return false; }
        }

        public async Task<Result<string>> SaveFile(IFormFile file, int userId)
        {
            try {
                if (file.Length == 0) return Result<string>.Fail("El archivo no existe");

                string fileName = Path.GetFileName(file.FileName);
                string supabasePath = $"{userId}/{fileName}";

                var existing = await IsFileExists(supabasePath);
                if (existing)
                {
                    fileName = $"{DateTime.Now.Ticks}_{fileName}";
                    supabasePath = $"{userId}/{fileName}";
                }

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                await _supabase.Storage
                    .From(BucketName)
                    .Upload(ms.ToArray(), supabasePath, new Supabase.Storage.FileOptions
                    {
                        Upsert = true
                    });

                var url = await _supabase.Storage
                    .From(BucketName)
                    .Upload(ms.ToArray(), supabasePath);

                return Result<string>.Ok(url);
            } catch (Exception e)
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
