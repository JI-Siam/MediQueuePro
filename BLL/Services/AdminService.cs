using AutoMapper;
using BLL;
using DAL.EF.Tables;

public class AdminService
{
    AdminRepo repo;
    private readonly DoctorRepo doctorRepo;
    private readonly PatientRepo patientRepo;

    public AdminService(AdminRepo repo, DoctorRepo doctorRepo, PatientRepo patientRepo)
    {
        this.repo = repo;
        this.doctorRepo = doctorRepo;
        this.patientRepo = patientRepo;
    }

    private readonly Mapper mapper = MapperConfig.GetMapper();

    public AdminDTO Add(AdminRegDTO dto)
    {
        var admin = mapper.Map<Admin>(dto);
        // store plain password for now
        admin.PasswordHash = dto.Password;
        admin.CreatedAt = DateTime.Now;

        repo.Add(admin);
        return mapper.Map<AdminDTO>(admin);
    }

    public List<AdminDTO> GetAll()
    {
        return mapper.Map<List<AdminDTO>>(repo.GetAll());
    }

    public AdminDTO? GetById(int id)
    {
        var a = repo.GetById(id);
        return a == null ? null : mapper.Map<AdminDTO>(a);
    }

    public AdminDTO? Update(int id, AdminDTO dto)
    {
        var existing = repo.GetById(id);
        if (existing == null) return null;

        existing.FullName = dto.FullName;
        existing.Phone = dto.Phone;

        repo.Update(existing);
        return mapper.Map<AdminDTO>(existing);
    }

    public bool Delete(int id)
    {
        return repo.Delete(id);
    }

    public AdminAnalyticsDashboardDTO GetAnalyticsDashboard()
    {
        var appointments = repo.GetAppointmentsWithDetails();
        return BuildAnalyticsDashboard(appointments);
    }

    public List<AdminPatientAnalyticsDTO> GetUniquePatients(int? doctorId = null, int? specializationId = null)
    {
        var appointments = repo.GetAppointmentsWithDetails();

        if (doctorId.HasValue)
        {
            appointments = appointments.Where(appointment => appointment.DoctorId == doctorId.Value).ToList();
        }

        if (specializationId.HasValue)
        {
            appointments = appointments
                .Where(appointment => appointment.Doctor?.SpecializationId == specializationId.Value)
                .ToList();
        }

        return BuildPatientAnalytics(appointments);
    }

    public AdminDTO? Login(LoginDTO dto)
    {
        var admin = repo.GetByEmail(dto.Email);
        if (admin == null) return null;

        if (!string.Equals(admin.PasswordHash, dto.Password, StringComparison.Ordinal))
        {
            return null;
        }

        return mapper.Map<AdminDTO>(admin);
    }

    private AdminAnalyticsDashboardDTO BuildAnalyticsDashboard(List<Appointment> appointments)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var summary = new AdminSummaryStatsDTO
        {
            TotalDoctors = doctorRepo.GetAll().Count,
            TotalPatients = patientRepo.GetAll().Count,
            TotalAppointments = appointments.Count,
            TotalVisits = appointments.Count(appointment => appointment.IsVisited == true),
            TodayVisits = appointments.Count(appointment => appointment.AppointmentDate == today && appointment.IsVisited == true)
        };

        var doctorStats = appointments
            .Where(appointment => appointment.Doctor != null)
            .GroupBy(appointment => new
            {
                appointment.DoctorId,
                appointment.Doctor!.FullName,
                appointment.Doctor.SpecializationId,
                SpecializationName = appointment.Doctor.Specialization.Name
            })
            .Select(group => new AdminDoctorAnalyticsDTO
            {
                DoctorId = group.Key.DoctorId,
                DoctorName = group.Key.FullName,
                SpecializationName = group.Key.SpecializationName,
                TotalBookings = group.Count(),
                TotalVisits = group.Count(appointment => appointment.IsVisited == true),
                TodayBookings = group.Count(appointment => appointment.AppointmentDate == today),
                TodayVisits = group.Count(appointment => appointment.AppointmentDate == today && appointment.IsVisited == true),
                UniquePatients = group.Select(appointment => appointment.PatientId).Distinct().Count()
            })
            .OrderByDescending(stat => stat.TotalBookings)
            .ToList();

        var specializationStats = appointments
            .Where(appointment => appointment.Doctor?.Specialization != null)
            .GroupBy(appointment => new
            {
                appointment.Doctor!.SpecializationId,
                SpecializationName = appointment.Doctor.Specialization.Name
            })
            .Select(group => new AdminSpecializationAnalyticsDTO
            {
                SpecializationId = group.Key.SpecializationId,
                SpecializationName = group.Key.SpecializationName,
                DoctorsCount = group.Select(appointment => appointment.DoctorId).Distinct().Count(),
                TotalBookings = group.Count(),
                TotalVisits = group.Count(appointment => appointment.IsVisited == true),
                UniquePatients = group.Select(appointment => appointment.PatientId).Distinct().Count()
            })
            .OrderByDescending(stat => stat.TotalBookings)
            .ToList();

        return new AdminAnalyticsDashboardDTO
        {
            Summary = summary,
            Doctors = doctorStats,
            Specializations = specializationStats,
            UniquePatients = BuildPatientAnalytics(appointments)
        };
    }

    private List<AdminPatientAnalyticsDTO> BuildPatientAnalytics(IEnumerable<Appointment> appointments)
    {
        return appointments
            .Where(appointment => appointment.Patient != null)
            .GroupBy(appointment => appointment.PatientId)
            .Select(group =>
            {
                var latest = group
                    .OrderByDescending(appointment => appointment.AppointmentDate)
                    .ThenByDescending(appointment => appointment.AppointmentTime)
                    .First();

                return new AdminPatientAnalyticsDTO
                {
                    PatientId = group.Key,
                    FullName = latest.Patient.FullName,
                    Email = latest.Patient.Email,
                    Phone = latest.Patient.Phone,
                    Age = latest.Patient.Age,
                    Gender = latest.Patient.Gender,
                    BloodGroup = latest.Patient.BloodGroup,
                    Address = latest.Patient.Address,
                    TotalBookings = group.Count(),
                    TotalVisits = group.Count(appointment => appointment.IsVisited == true),
                    LastAppointmentDate = latest.AppointmentDate,
                    LastDoctorName = latest.Doctor?.FullName,
                    LastStatus = latest.Status
                };
            })
            .OrderByDescending(patient => patient.TotalBookings)
            .ThenBy(patient => patient.FullName)
            .ToList();
    }
}