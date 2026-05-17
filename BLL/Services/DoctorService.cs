using AutoMapper;
using BLL;
using DAL.EF.Tables;

public class DoctorService
{
    private readonly DoctorRepo repo;
    private readonly SpecializationRepo specializationRepo;
    private readonly Mapper mapper;

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

        // plain-text comparison for now
        if (!string.Equals(doctor.PasswordHash, dto.Password, StringComparison.Ordinal))
        {
            return null;
        }

        return mapper.Map<DoctorDTO>(doctor);
    }

    public DoctorDTO Add(DoctorRegDTO dto)
    {
        var doctor = mapper.Map<Doctor>(dto);
        // store plain password temporarily
        doctor.PasswordHash = dto.Password;
        doctor.CreatedAt = DateTime.Now;
        doctor.IsAvailable = dto.IsAvailable ?? true;

        repo.Add(doctor);
        return mapper.Map<DoctorDTO>(doctor);
    }

    public DoctorDTO? Update(int id, DoctorDTO dto)
    {
        var existing = repo.GetById(id);
        if (existing == null) return null;

        existing.FullName = dto.FullName;
        existing.Phone = dto.Phone;
        existing.SpecializationId = dto.SpecializationId;
        existing.VisitingStartTime = dto.VisitingStartTime;
        existing.VisitingEndTime = dto.VisitingEndTime;
        existing.Honorarium = dto.Honorarium;
        existing.ExperienceYears = dto.ExperienceYears;
        existing.IsAvailable = dto.IsAvailable;

        repo.Update(existing);
        return mapper.Map<DoctorDTO>(existing);
    }

    public bool Delete(int id)
    {
        return repo.Delete(id);
    }

    public List<DoctorDTO> GetAll()
    {
        var doctors = repo.GetAll();
        return mapper.Map<List<DoctorDTO>>(doctors);
    }

    public List<DoctorDTO> GetFiltered(int? specializationId, bool? isAvailable)
    {
        var doctors = repo.GetFiltered(specializationId, isAvailable);
        return mapper.Map<List<DoctorDTO>>(doctors);
    }

    public List<Specialization> GetSpecializations()
    {
        return specializationRepo.GetAll();
    }

    public DoctorDTO? GetById(int doctorId)
    {
        var doctor = repo.GetById(doctorId);
        return doctor == null ? null : mapper.Map<DoctorDTO>(doctor);
    }

    // Note: password hashing removed per request; plain passwords are stored in the PasswordHash column temporarily.
}