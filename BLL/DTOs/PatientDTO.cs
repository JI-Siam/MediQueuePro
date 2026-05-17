public class PatientDTO
{
    public int PatientId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? BloodGroup { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }
}