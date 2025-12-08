using Microsoft.AspNetCore.Mvc;

namespace SetelaServerV3._1.Shared.Utilities
{
    public static class ResultExtension
    {
        public static ActionResult<T> ToActionResult<T>(this Result<T> result)
        {
            if (result.Success)
            {
                return new OkObjectResult(result.Value);
            }

            return new ObjectResult(new { error = result.Error }) { StatusCode = result.StatusCode };
        }
    }
}
