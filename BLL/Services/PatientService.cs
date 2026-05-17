using AutoMapper;
using BLL;
using DAL.EF.Tables;
using System.Security.Cryptography;
using System.Text;

public class PatientService
{
    private readonly PatientRepo repo;
    private static readonly IMapper mapper = MapperConfig.GetMapper();

    public PatientService(PatientRepo repo)
    {
        this.repo = repo;
    }

    public PatientDTO? Register(PatientRegDTO dto)
    {
        if (repo.GetByEmail(dto.Email) != null)
        {
            return null;
        }

        var patient = mapper.Map<Patient>(dto);
        patient.PasswordHash = HashPassword(dto.Password);
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

        var hashedPassword = HashPassword(dto.Password);

        if (!string.Equals(patient.PasswordHash, hashedPassword, StringComparison.Ordinal))
        {
            return null;
        }

        return mapper.Map<PatientDTO>(patient);
    }

    private static string HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = SHA256.HashData(passwordBytes);
        return Convert.ToHexString(hashBytes);
    }
}