using AutoMapper;
using BLL;
using DAL.EF.Tables;


public class PatientService
{
    PatientRepo repo;
    Mapper mapper;

    public PatientService(PatientRepo repo)
    {
        this.repo = repo;
        mapper = MapperConfig.GetMapper();
    }

    public PatientDTO? Register(PatientRegDTO dto)
    {
        if (repo.GetByEmail(dto.Email) != null)
        {
            return null;
        }

        var patient = mapper.Map<Patient>(dto);
        patient.PasswordHash = dto.Password;
        patient.CreatedAt = DateTime.Now;

        repo.Add(patient);
        return mapper.Map<PatientDTO>(patient);
    }

    public PatientDTO? Login(LoginDTO dto)
    {
        var patient = repo.GetByEmail(dto.Email);

        if (patient == null)
        {
            return null;
        }

        if (!string.Equals(patient.PasswordHash, dto.Password, StringComparison.Ordinal))
        {
            return null;
        }
        return mapper.Map<PatientDTO>(patient);
    }


    public List<PatientDTO> GetAll()
    {
        return mapper.Map<List<PatientDTO>>(repo.GetAll());
    }

    public PatientDTO? GetById(int id)
    {
        var p = repo.GetById(id);
        return p == null ? null : mapper.Map<PatientDTO>(p);
    }

    public PatientDTO? Update(int id, PatientDTO dto)
    {
        var existing = repo.GetById(id);
        if (existing == null) return null;


        existing.FullName = dto.FullName;
        existing.Phone = dto.Phone;
        existing.Age = dto.Age;
        existing.Gender = dto.Gender;
        existing.BloodGroup = dto.BloodGroup;
        existing.Address = dto.Address;

        repo.Update(existing);
        return mapper.Map<PatientDTO>(existing);
    }

    public PatientRegDTO? GetEditModel(int id)
    {
        var p = repo.GetById(id);
        if (p == null) return null;

        return new PatientRegDTO
        {
            FullName = p.FullName,
            Email = p.Email,
            Password = p.PasswordHash,
            Phone = p.Phone,
            Age = p.Age,
            Gender = p.Gender,
            BloodGroup = p.BloodGroup,
            Address = p.Address
        };
    }

    public PatientDTO? UpdateFromReg(int id, PatientRegDTO dto)
    {
        var existing = repo.GetById(id);
        if (existing == null) return null;

        existing.FullName = dto.FullName;
        existing.Email = dto.Email;
        existing.PasswordHash = dto.Password;
        existing.Phone = dto.Phone;
        existing.Age = dto.Age;
        existing.Gender = dto.Gender;
        existing.BloodGroup = dto.BloodGroup;
        existing.Address = dto.Address;

        repo.Update(existing);
        return mapper.Map<PatientDTO>(existing);
    }

    public bool Delete(int id)
    {
        return repo.Delete(id);
    }

    public List<DoctorDTO> GetAllDoctor()
    {
        var doctors = repo.GetAllDoctor();
        return mapper.Map<List<DoctorDTO>>(doctors);
    }

    public DoctorDTO? GetDoctorById(int doctorId)
    {
        var doctor = repo.GetAllDoctor().FirstOrDefault(doctor => doctor.DoctorId == doctorId);
        return doctor == null ? null : mapper.Map<DoctorDTO>(doctor);
    }
}