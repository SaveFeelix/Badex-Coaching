#nullable disable
using Microsoft.EntityFrameworkCore;
using Server.Db.Models;

namespace Server.Db;

public class DataContext : DbContext
{
    public ILogger<DataContext> Logger { get; }

    public DataContext(DbContextOptions options, ILogger<DataContext> logger) : base(options)
    {
        Logger = logger;
    }

    public DbSet<UserModel> User { get; set; }

    public async Task Init()
    {
        await Database.MigrateAsync();
        if (!await User.AnyAsync())
        {
            UserModel user =
                new("Administrator", "Administrator", null, null, "administrator", true, true);
            var generatedPassword = await user.GeneratePassword();
            await User.AddAsync(user);
            await SaveChangesAsync();
            await File.WriteAllTextAsync("password.txt", generatedPassword);
        }

        if (!File.Exists("password.txt"))
        {
            Logger.LogInformation("Cannot read password!");
            return;
        }

        var password = await File.ReadAllTextAsync("password.txt");
        Logger.LogInformation("{Password}", password);
    }
}