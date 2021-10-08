namespace Domain.Exceptions
{
    public class GeneralError
    {
        public string code { get; set; } = "internal_server_error";
        public string message { get; set; } = "Có lỗi xảy ra trong quá trình xử lý, vui lòng thử lại!";
    }
}