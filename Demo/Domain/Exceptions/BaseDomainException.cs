using Newtonsoft.Json;
using System;
using System.Net;

namespace Domain.Exceptions
{
    public class BaseDomainException : Exception
    {
        public string code { get; set; }

        public int status { get; set; }

        public BaseDomainException(string code, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            this.code = code;
            status = (int)statusCode;
        }

        public virtual dynamic GetMessage() => new { code, message = Message };

        public override string ToString()
        {
            return JsonConvert.SerializeObject(GetMessage(), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}