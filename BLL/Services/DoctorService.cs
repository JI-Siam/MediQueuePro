using AutoMapper;
using BLL;
using DAL.EF.Tables;

public class DoctorService
{
    DoctorRepo repo;
    SpecializationRepo specializationRepo;
    Mapper mapper;

    public DoctorService(DoctorRepo repo, SpecializationRepo specializationRepo)
    {
        this.repo = repo;
        this.specializationRepo = specializationRepo;
        mapper = MapperConfig.GetMapper();
    }

    public DoctorDTO? Login(LoginDTO dto)
    {
        var doctor = repo.GetByEmail(dto.Email);

        if (doctor == null)
        {
            return null;
        }

        if (!string.Equals(doctor.PasswordHash, dto.Password))
        {
            return null;
        }

        var data = mapper.Map<DoctorDTO>(doctor);

        return data;
    }

    public DoctorDTO Add(DoctorRegDTO dto)

    {
        var doctor = mapper.Map<Doctor>(dto);
        doctor.PasswordHash = dto.Password;
        doctor.CreatedAt = DateTime.Now;
        doctor.IsAvailable = dto.IsAvailable ?? true;

        repo.Add(doctor);
        var addedDoctor = mapper.Map<DoctorDTO>(doctor);
        return addedDoctor;
    }

    public List<DoctorDTO> GetAll()
    {
        var doctors = repo.GetAll();
        return mapper.Map<List<DoctorDTO>>(doctors);
    }

    public DoctorDTO? GetById(int doctorId)
    {
        var doctor = repo.GetById(doctorId);
        return doctor == null ? null : mapper.Map<DoctorDTO>(doctor);
    }

    public List<DoctorDTO> GetFiltered(int? specializationId, bool? isAvailable)
    {
        var doctors = repo.GetFiltered(specializationId, isAvailable);
        return mapper.Map<List<DoctorDTO>>(doctors);
    }

    public List<Specialization> GetSpecializations()
    {
        var specializations = specializationRepo.GetAll();
        return specializations;
    }
    public DoctorRegDTO? GetEditModel(int id)
    {
        var doctor = repo.GetById(id);
        if (doctor == null) return null;

        var editableData = new DoctorRegDTO
        {
            FullName = doctor.FullName,
            Email = doctor.Email,
            Phone = doctor.Phone,
            SpecializationId = doctor.SpecializationId,
            VisitingStartTime = doctor.VisitingStartTime,
            VisitingEndTime = doctor.VisitingEndTime,
            Honorarium = doctor.Honorarium,
            ExperienceYears = doctor.ExperienceYears,
            IsAvailable = doctor.IsAvailable
        };

        return editableData;
    }

    public DoctorDTO? Update(int id, DoctorRegDTO dto)
    {
        var existing = repo.GetById(id);
        if (existing == null) return null;

        existing.FullName = dto.FullName;
        existing.Email = dto.Email;
        existing.PasswordHash = dto.Password;
        existing.Phone = dto.Phone;
        existing.SpecializationId = dto.SpecializationId;
        existing.VisitingStartTime = dto.VisitingStartTime;
        existing.VisitingEndTime = dto.VisitingEndTime;
        existing.Honorarium = dto.Honorarium;
        existing.ExperienceYears = dto.ExperienceYears;
        existing.IsAvailable = dto.IsAvailable;

        repo.Update(existing);
        var updatedDoctor = mapper.Map<DoctorDTO>(existing);

        return updatedDoctor;
    }

    public bool Delete(int id)
    {
        return repo.Delete(id);
    }






}