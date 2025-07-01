using Microsoft.AspNetCore.DataProtection.Repositories;
using System;
using YoloSoccerApp.Data;

var builder = WebApplication.CreateBuilder(args);

//connection string
string connecitonString =
    builder.Configuration.GetConnectionString("yolo") ?? throw new ArgumentException(nameof(connecitonString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserRepository>(sp=>
new UserSqlRepository(connecitonString, sp.GetRequiredService<ILogger<UserSqlRepository>>()));

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
