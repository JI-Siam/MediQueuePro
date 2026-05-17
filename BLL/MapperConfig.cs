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
            cfg.CreateMap<Patient, DoctorDTO>().ReverseMap();
        });
        public static Mapper GetMapper()
        {
            return new Mapper(config);
        }
    }
}