namespace Api.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public string? VerificationToken { get; set; }
        public bool? EmailConfirmed { get; set; } = false;
        public string? PasswordResetToken { get; set; } = string.Empty;
        public DateTime? ResetTokenExpiresAt { get; set; } = null;
    }
}
