using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

public class SpecializationRepo
{
    MediQueueProDbContext db;

    public SpecializationRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }

    public List<Specialization> GetAll()
    {
        return db.Specializations
            .OrderBy(specialization => specialization.Name)
            .ToList();
    }
}