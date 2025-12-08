using Microsoft.AspNetCore.Http;

namespace SetelaServerV3._1.Shared.Utilities
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public T? Value { get; private set; }
        public string? Error { get; private set; }
        public int StatusCode { get; init; }

        private Result() { }


        public static Result<T> Ok(T value) => new Result<T> { Success = true, Value = value, StatusCode = 200 };
        public static Result<T> Fail(string error, int statusCode = 400) => new Result<T> { Success = false, Error = error, StatusCode = statusCode };
    }
}
