using AutoMapper;
using BLL;
using DAL.EF.Tables;

public class AppointMentService
{
    private readonly AppointMentRepo repo;
    private readonly Mapper mapper;

    public AppointMentService(AppointMentRepo repo)
    {
        this.repo = repo;
        mapper = MapperConfig.GetMapper();
    }

    public AppointmentDTO? GetById(int appointmentId)
    {
        var appointment = repo.GetById(appointmentId);
        return appointment == null ? null : mapper.Map<AppointmentDTO>(appointment);
    }

    public List<AppointmentDTO> GetByPatientId(int patientId)
    {
        return mapper.Map<List<AppointmentDTO>>(repo.GetByPatientId(patientId));
    }

    public List<AppointmentDTO> GetByDoctorId(int doctorId)
    {
        return mapper.Map<List<AppointmentDTO>>(repo.GetByDoctorId(doctorId));
    }

    public List<AppointmentDTO> GetQueueByDoctorId(int doctorId)
    {
        // default to today's date; repository will return an already-ordered queue
        var date = DateOnly.FromDateTime(DateTime.Today);
        return mapper.Map<List<AppointmentDTO>>(repo.GetQueueByDoctorId(doctorId, date));
    }

    public List<AppointmentDTO> GetPatientQueues(int patientId)
    {
        // Let repository/database return the properly filtered and ordered queue list
        var date = DateOnly.FromDateTime(DateTime.Today);
        return mapper.Map<List<AppointmentDTO>>(repo.GetQueueByPatientId(patientId, date));
    }

    public AppointmentDTO? Book(int patientId, AppointmentCreateDTO dto)
    {
        if (repo.HasPatientAppointmentOnDate(patientId, dto.DoctorId, dto.AppointmentDate))
        {
            return null;
        }

        var appointment = mapper.Map<Appointment>(dto);
        var confirmedAtUtc = DateTime.UtcNow;

        appointment.PatientId = patientId;
        appointment.AppointmentTime = TimeOnly.FromDateTime(confirmedAtUtc);
        appointment.QueueToken = repo.GetNextQueueToken(dto.DoctorId, dto.AppointmentDate);
        appointment.Status = "Pending";
        appointment.IsVisited = false;
        appointment.CreatedAt = confirmedAtUtc;
        appointment.IsEmergency = dto.IsEmergency;

        repo.Add(appointment);

        var savedAppointment = repo.GetById(appointment.AppointmentId) ?? appointment;
        return mapper.Map<AppointmentDTO>(savedAppointment);
    }

    public AppointmentDTO? UpdateAppointmentStatus(int appointmentId, string status)
    {
        var updated = repo.UpdateStatus(appointmentId, status);
        return updated == null ? null : mapper.Map<AppointmentDTO>(updated);
    }
}