namespace Api.Dtos
{
    public class LoginReqestDto
    {
        public bool Succeeded
        {
            get; set;
        }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public List<String> ?Roles { get; set; }
    }
}
