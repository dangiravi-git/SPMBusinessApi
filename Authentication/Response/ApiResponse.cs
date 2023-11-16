namespace Authentication.Response
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public ErrorType ErrorCode { get; set; }
        public string? Message { get; set; }
    }
}
