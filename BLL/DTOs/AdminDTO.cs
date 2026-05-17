public class AdminDTO
{
    public int AdminId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public DateTime? CreatedAt { get; set; }
}
