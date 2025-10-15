using Tech.Core.Auth.Enums;

namespace Tech.Core.Auth.Common.Exceptions
{
    public class ServiceException : Exception
    {
        public ErrorType Type { get; }
        public ServiceException(string message, ErrorType type) : base(message)
        {
            Type = type;
        }
    }
}
