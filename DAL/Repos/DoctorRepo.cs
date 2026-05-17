using DAL.EF;

public class DoctorRepo
{
    MediQueueProDbContext db;

    public DoctorRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }
}