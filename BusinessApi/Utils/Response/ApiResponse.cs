namespace BusinessApi.Utils.Response
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public ErrorType ErrorCode { get; set; }
        public string? Message { get; set; }
        public T Item { get; set; }
    }
}
