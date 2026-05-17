using System.Data.Common;

public class AdminService
{
    AdminRepo repo;

    public AdminService(AdminRepo repo)
    {
        this.repo = repo;
    }
}