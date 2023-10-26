namespace BusinessApi.Utils.Response
{
    public class ApiResponse
    {
        public bool success { get; set; }
        public ErrorType error_code { get; set; }
        public string? message { get; set; }
        public dynamic? data { get; set; }
    }
}
