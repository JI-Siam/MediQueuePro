public class AppointmentDTO
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public string DoctorName { get; set; } = string.Empty;

    public string? DoctorPhone { get; set; }

    public int SpecializationId { get; set; }

    public TimeOnly VisitingStartTime { get; set; }

    public TimeOnly VisitingEndTime { get; set; }

    public decimal? Honorarium { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly AppointmentTime { get; set; }

    public int QueueToken { get; set; }

    public bool IsEmergency { get; set; }

    public bool IsVisited { get; set; }

    public DateTime? VisitTime { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; }
}
