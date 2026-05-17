using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public int SpecializationId { get; set; }

    public TimeOnly VisitingStartTime { get; set; }

    public TimeOnly VisitingEndTime { get; set; }

    public decimal? Honorarium { get; set; }

    public int? ExperienceYears { get; set; }

    public bool? IsAvailable { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Specialization Specialization { get; set; } = null!;
}
