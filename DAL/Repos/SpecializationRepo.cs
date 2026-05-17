using DAL.EF;

public class SpecializationRepo
{
    MediQueueProDbContext db;

    public SpecializationRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }
}