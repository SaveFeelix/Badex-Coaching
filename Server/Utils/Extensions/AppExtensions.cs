using System.Text;
using Microsoft.EntityFrameworkCore;
using Server.Db;
using Server.Settings;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

    public static bool AddJwt(this WebApplicationBuilder builder)
    {
        try
        {
#if DEBUG
            JwtSettings settings =
                new JwtSettings("y$GLB%&PyidUVEpSnMwwfT96v4sT42T!kMvUTrrqUvRt8x385!6B&nT3mN%e36cg", 365);
#else
            string? token = Environment.GetEnvironmentVariable("JWT_TOKEN");
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("JWT_TOKEN environment variable not set");
                return false;
            }
            string? envDuration = Environment.GetEnvironmentVariable("JWT_DURATION");
            if (string.IsNullOrEmpty(envDuration) || !double.TryParse(envDuration, out double duration))
            {
                Console.WriteLine("JWT_DURATION environment variable not set");
                return false;
            }
            JwtSettings settings = new JwtSettings(token, duration);
#endif
            builder.Services.AddSingleton(settings);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    public static bool AddSwagger(this WebApplicationBuilder builder)
    {
        try
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Badex-Coaching",
                    License = new OpenApiLicense
                    {
                        Name = "Apache 2.0",
                        Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}