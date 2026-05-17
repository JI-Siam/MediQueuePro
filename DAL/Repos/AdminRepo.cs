using DAL.EF;
using DAL.EF.Tables;

public class AdminRepo
{
    MediQueueProDbContext db;

    public AdminRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }

    public Admin Add(Admin admin)
    {
        db.Admins.Add(admin);
        db.SaveChanges();
        return admin;
    }

    public List<Admin> GetAll()
    {
        return db.Admins.OrderBy(a => a.FullName).ToList();
    }

    public Admin? GetById(int id)
    {
        return db.Admins.FirstOrDefault(a => a.AdminId == id);
    }

    public Admin Update(Admin admin)
    {
        db.Admins.Update(admin);
        db.SaveChanges();
        return admin;
    }

    public bool Delete(int id)
    {
        var a = db.Admins.FirstOrDefault(x => x.AdminId == id);
        if (a == null) return false;
        db.Admins.Remove(a);
        db.SaveChanges();
        return true;
    }

}