using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Shared.Common.Interfaces
{
    public interface IFileUploadService
    {
        public Task<Result<object>> UploadFile(List<IFormFile> files);
    }
}
