namespace Api.Models
{
    public class Hash
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public string? VerificationToken { get; set; }
        public string? PasswordResetToken { get; set; } = string.Empty;
    }
}
