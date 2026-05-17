using System.ComponentModel.DataAnnotations;

public class DoctorRegDTO
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public string? Phone { get; set; }

    [Required]
    public int SpecializationId { get; set; }

    [Required]
    public TimeOnly VisitingStartTime { get; set; }

    [Required]
    public TimeOnly VisitingEndTime { get; set; }

    public decimal? Honorarium { get; set; }

    public int? ExperienceYears { get; set; }

    public bool? IsAvailable { get; set; }
}
