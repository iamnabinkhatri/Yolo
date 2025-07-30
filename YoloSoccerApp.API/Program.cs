
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using YoloSoccerApp.API.Services;
using YoloSoccerApp.Data;
using YoloSoccerApp.Logic;

var builder = WebApplication.CreateBuilder(args);

//connection string
string connectionString = builder.Configuration.GetConnectionString("yolo") ?? throw new ArgumentException(nameof(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddSingleton<JwtSettings>(sp =>
    sp.GetRequiredService<IOptions<JwtSettings>>().Value);

builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["access_token"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IUserRoleRepository>(sp =>
new UserRoleSqlRepository(connectionString, sp.GetRequiredService<ILogger<UserRoleSqlRepository>>()));

builder.Services.AddSingleton<IUserRepository>(sp =>
new UserSqlRepository(connectionString, sp.GetRequiredService<ILogger<UserSqlRepository>>()));

builder.Services.AddSingleton<IUserLoginRepository>(sp =>
new UserLoginSqlRepository(connectionString, sp.GetRequiredService<ILogger<UserLoginSqlRepository>>()));

builder.Services.AddSingleton<IPlayerRoleRepository>(sp =>
new PlayerRoleSqlRepository(connectionString, sp.GetRequiredService<ILogger<PlayerRoleSqlRepository>>()));

builder.Services.AddSingleton<IPlayerRepository>(sp =>
new PlayerSqlRepository(connectionString, sp.GetRequiredService<ILogger<PlayerSqlRepository>>()));

builder.Services.AddSingleton<IPlayerStaticsRepository>(sp =>
new PlayerStaticsSqlRepository(connectionString, sp.GetRequiredService<ILogger<PlayerStaticsSqlRepository>>()));

builder.Services.AddSingleton<IPollRepository>(sp =>
new PollSqlRepository(connectionString, sp.GetRequiredService<ILogger<PollSqlRepository>>()));

builder.Services.AddSingleton<IPollOptionRepository>(sp =>
new PollOptionSqlRepository(connectionString, sp.GetRequiredService<ILogger<PollOptionSqlRepository>>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
