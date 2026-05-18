public class AdminAnalyticsDashboardDTO
{
    public AdminSummaryStatsDTO Summary { get; set; } = new();

    public List<AdminDoctorAnalyticsDTO> Doctors { get; set; } = new();

    public List<AdminSpecializationAnalyticsDTO> Specializations { get; set; } = new();

    public List<AdminPatientAnalyticsDTO> UniquePatients { get; set; } = new();
}

public class AdminSummaryStatsDTO
{
    public int TotalDoctors { get; set; }

    public int TotalPatients { get; set; }

    public int TotalAppointments { get; set; }

    public int TotalVisits { get; set; }

    public int TodayVisits { get; set; }
}

public class AdminDoctorAnalyticsDTO
{
    public int DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public string SpecializationName { get; set; } = string.Empty;

    public int TotalBookings { get; set; }

    public int TotalVisits { get; set; }

    public int TodayBookings { get; set; }

    public int TodayVisits { get; set; }

    public int UniquePatients { get; set; }
}

public class AdminSpecializationAnalyticsDTO
{
    public int SpecializationId { get; set; }

    public string SpecializationName { get; set; } = string.Empty;

    public int DoctorsCount { get; set; }

    public int TotalBookings { get; set; }

    public int TotalVisits { get; set; }

    public int UniquePatients { get; set; }
}

public class AdminPatientAnalyticsDTO
{
    public int PatientId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? BloodGroup { get; set; }

    public string? Address { get; set; }

    public int TotalBookings { get; set; }

    public int TotalVisits { get; set; }

    public DateOnly? LastAppointmentDate { get; set; }

    public string? LastDoctorName { get; set; }

    public string? LastStatus { get; set; }
}