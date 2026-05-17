using DAL.EF;

public class PatientRepo
{
    MediQueueProDbContext db;

    public PatientRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }
}