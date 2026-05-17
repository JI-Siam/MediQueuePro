using DAL.EF;

public class AdminRepo
{
    MediQueueProDbContext db;

    public AdminRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }

}