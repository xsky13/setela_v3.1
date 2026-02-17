using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Shared.Common.Interfaces
{
    public interface IFileStorage
    {
        Task<Result<string>> SaveFile(IFormFile file, int userId);
        Task<Result<object>> DeleteFile(string fileName, int userId);
    }
}
