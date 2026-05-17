using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly AppointmentTime { get; set; }

    public int QueueToken { get; set; }

    public bool? IsEmergency { get; set; }

    public bool? IsVisited { get; set; }

    public DateTime? VisitTime { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
