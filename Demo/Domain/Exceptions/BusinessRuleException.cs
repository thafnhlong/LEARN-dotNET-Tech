using System.Net;

namespace Domain.Exceptions
{
    public class BusinessRuleException : BaseDomainException
    {
        public BusinessRuleException(string code, string message) : base(code, message)
        {
        }

        public BusinessRuleException(string code, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(code, message, statusCode)
        {
        }

        public override dynamic GetMessage() => new { code, message = Message };
    }
}