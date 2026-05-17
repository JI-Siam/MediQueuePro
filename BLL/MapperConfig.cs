using AutoMapper;
using DAL.EF.Tables;

namespace BLL
{
    public class MapperConfig
    {
        public static MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Patient, PatientDTO>().ReverseMap();
            cfg.CreateMap<PatientRegDTO, Patient>()
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());
            cfg.CreateMap<Doctor, DoctorDTO>()
                .ForMember(dest => dest.SpecializationName, opt => opt.MapFrom(src => src.Specialization.Name))
                .ReverseMap()
                .ForMember(dest => dest.Specialization, opt => opt.Ignore());
            cfg.CreateMap<DoctorRegDTO, Doctor>()
                .ForMember(dest => dest.DoctorId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.Specialization, opt => opt.Ignore());
            cfg.CreateMap<Admin, AdminDTO>().ReverseMap();
            cfg.CreateMap<AdminRegDTO, Admin>()
                .ForMember(dest => dest.AdminId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            cfg.CreateMap<AppointmentCreateDTO, Appointment>()
                .ForMember(dest => dest.AppointmentId, opt => opt.Ignore())
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.QueueToken, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.VisitTime, opt => opt.Ignore())
                .ForMember(dest => dest.IsVisited, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore());
            cfg.CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.DoctorPhone, opt => opt.MapFrom(src => src.Doctor.Phone))
                .ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src => src.Doctor.SpecializationId))
                .ForMember(dest => dest.VisitingStartTime, opt => opt.MapFrom(src => src.Doctor.VisitingStartTime))
                .ForMember(dest => dest.VisitingEndTime, opt => opt.MapFrom(src => src.Doctor.VisitingEndTime))
                .ForMember(dest => dest.Honorarium, opt => opt.MapFrom(src => src.Doctor.Honorarium))
                .ForMember(dest => dest.IsEmergency, opt => opt.MapFrom(src => src.IsEmergency ?? false))
                .ForMember(dest => dest.IsVisited, opt => opt.MapFrom(src => src.IsVisited ?? false));
        });
        public static Mapper GetMapper()
        {
            return new Mapper(config);
        }
    }
}