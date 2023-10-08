using Microsoft.EntityFrameworkCore;

namespace Server.Db;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public async Task Init()
    {
        await Database.MigrateAsync();
    }
}