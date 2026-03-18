namespace Api.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        
        public bool? EmailConfirmed { get; set; } = false;
        
        public DateTime? ResetTokenExpiresAt { get; set; } = null;
    }
}
