using Microsoft.AspNetCore.DataProtection.Repositories;
using YoloSoccerApp.Data;

var builder = WebApplication.CreateBuilder(args);

//connection string
string connectionString = builder.Configuration.GetConnectionString("yolo") ?? throw new ArgumentException(nameof(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserRoleRepository>(sp =>
new UserRoleSqlRepository(connectionString, sp.GetRequiredService<ILogger<UserRoleSqlRepository>>()));

builder.Services.AddSingleton<IUserRepository>(sp =>
new UserSqlRepository(connectionString, sp.GetRequiredService<ILogger<UserSqlRepository>>()));

builder.Services.AddSingleton<IPlayerRepository>(sp =>
new PlayerSqlRepository(connectionString, sp.GetRequiredService<ILogger<PlayerSqlRepository>>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
