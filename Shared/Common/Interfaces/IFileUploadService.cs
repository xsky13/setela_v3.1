using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Shared.Common.Interfaces
{
    public interface IFileUploadService
    {
        public Task<Result<object>> UploadFiles(List<IFormFile> files);
        public Task<Result<string>> UploadFile(IFormFile file);
    }
}
