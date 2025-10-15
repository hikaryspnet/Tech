using Tech.Core.Auth.Enums;

namespace Tech.Core.Auth.Common.Result
{
    public class Result<T>
    {
        public T? Value { get; private set; }
        public bool IsSuccess => Errors.Count == 0;
        public List<ResultError> Errors { get; private set; } = new();

        public static Result<T> Ok(T value) => new Result<T> { Value = value };
        public static Result<T> Fail(string message, ErrorType type = ErrorType.Internal)
            => new Result<T> { Errors = new List<ResultError> { new ResultError(message, type) } };
    }

    public class ResultError
    {
        public string Message { get; }
        public ErrorType Type { get; }

        public ResultError(string message, ErrorType type)
        {
            Message = message;
            Type = type;
        }
    }
}
