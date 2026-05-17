public class DoctorDTO
{
    public int DoctorId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public int SpecializationId { get; set; }

    public string SpecializationName { get; set; } = string.Empty;

    public TimeOnly VisitingStartTime { get; set; }

    public TimeOnly VisitingEndTime { get; set; }

    public decimal? Honorarium { get; set; }

    public int? ExperienceYears { get; set; }

    public bool? IsAvailable { get; set; }

    public DateTime? CreatedAt { get; set; }
}