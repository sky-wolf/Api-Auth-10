namespace Api.Models
{
    public class SystemRoles
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? Kode { get; set; }
    }
}
