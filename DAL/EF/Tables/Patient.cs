using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Patient
{
    public int PatientId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? BloodGroup { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
