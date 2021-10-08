using System.Net;

namespace Domain.Exceptions
{
    public class NotFoundRuleException : BaseDomainException
    {
        public NotFoundRuleException(string code, string message, HttpStatusCode statusCode = HttpStatusCode.NotFound) : base(code, message, statusCode)
        {
        }
    }
}