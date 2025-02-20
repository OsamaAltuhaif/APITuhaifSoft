using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TuhaifSoftAPI.Data;
using Microsoft.AspNetCore.Identity;
using TuhaifSoftAPI.EmilSender;
using TuhaifSoftAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TuhaifSoftAPIDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection"));
});

builder.Services.AddIdentityCore<Users>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<TuhaifSoftAPIDBContext>()
.AddDefaultTokenProviders();


Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
             .WriteTo.File("log/InformationUsersLog.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();
Log.Logger = new LoggerConfiguration().MinimumLevel.Warning()
             .WriteTo.File("log/WarningUsersLog.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))

    };
});

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddAuthorization();
builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ILogging, Logging>();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(); // للسماح بعرض الصور المخزنة في wwwroot
app.UseHttpsRedirection();


app.MapControllers();

app.Run();
