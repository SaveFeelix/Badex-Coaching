using Server.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);


if (!builder.AddDb())
    return;
if (!builder.AddJwt())
    return;
if (!builder.AddSwagger())
    return;

// Add services to the container.
builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Badex-Coaching v1");
        c.RoutePrefix = string.Empty;
    });
}

if (!await app.InitDb())
    return;
if (!app.AddSignalR())
    return;

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();