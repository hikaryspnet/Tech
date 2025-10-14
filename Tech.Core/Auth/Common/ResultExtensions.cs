using FluentResults;

namespace Tech.Application.Common
{
    public static class ResultExtensions
    {
        public static Result<T> ToResult<T>(this T value) => Result.Ok(value);
        public static Result Fail(string error) => Result.Fail(error);
    }
}
