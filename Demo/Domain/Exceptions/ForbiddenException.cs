namespace Domain.Exceptions
{
    public class ForbiddenException : BaseDomainException
    {
        public ForbiddenException(string code, string message) : base(code, message, System.Net.HttpStatusCode.Forbidden)
        {
        }
    }
}