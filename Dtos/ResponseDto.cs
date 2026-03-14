namespace Api.Dtos
{
    public class ResponseDto
    {
        public object? ResultData { get; set; }
        public bool? Success { get; set; }
        public string? Message { get; set; } = string.Empty;
    }
}
