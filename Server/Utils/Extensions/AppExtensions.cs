using Microsoft.EntityFrameworkCore;
using Server.Db;
#if DEBUG
using Microsoft.Data.Sqlite;

#else
using Npgsql;
#endif

namespace Server.Utils.Extensions;

public static class AppExtensions
{
    public static bool AddDb(this WebApplicationBuilder builder)
    {
        try
        {
#if DEBUG
            SqliteConnectionStringBuilder conBuilder = new SqliteConnectionStringBuilder()
            {
                Cache = SqliteCacheMode.Shared,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Pooling = true,
                DataSource = "Badex-Coaching.db",
                ForeignKeys = true,
                RecursiveTriggers = true,
                BrowsableConnectionString = true
            };
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(conBuilder.ToString())
                .UseLazyLoadingProxies(), ServiceLifetime.Singleton);
#else
            string? host = Environment.GetEnvironmentVariable("DB_HOST");
            if (string.IsNullOrEmpty(host))
            {
                Console.WriteLine("DB_HOST environment variable not set");
                return false;
            }
            string? password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("DB_PASSWORD environment variable not set");
                return false;
            }
            string? username = Environment.GetEnvironmentVariable("DB_USERNAME");
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("DB_USERNAME environment variable not set");
                return false;
            }

            NpgsqlConnectionStringBuilder conBuilder = new NpgsqlConnectionStringBuilder()
            {
                Database = "Badex-Coaching",
                BrowsableConnectionString = true,
                Pooling = true,
                Host = host,
                Multiplexing = true,
                Password = password,
                Port = 5432,
                Username = username,
                ApplicationName = "Badex-Coaching",
                SslMode = SslMode.Prefer,
                LogParameters = true,
                LoadBalanceHosts = true,
                TrustServerCertificate = true
            };
            builder.Services.AddDbContext<DataContext>(it => it.UseNpgsql(builder.ToString()).UseLazyLoadingProxies(), 
                ServiceLifetime.Singleton);
#endif
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public static async Task<bool> InitDb(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<DataContext>();
            await context.Init();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
    public static bool AddSignalR(this WebApplication app)
    {
        try
        {
            return true;
        } catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}