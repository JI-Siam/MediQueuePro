using DAL.EF;

public class AppointMentRepo
{

    MediQueueProDbContext db;

    public AppointMentRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }
}