using AutoMapper;
using BLL;
using DAL.EF.Tables;

public class AdminService
{
    AdminRepo repo;

    public AdminService(AdminRepo repo)
    {
        this.repo = repo;
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
}