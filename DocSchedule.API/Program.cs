using DocSchedule.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    )
);
builder.Services.AddControllers(); // 使用Controller架構

//跨網域資源共享，避免前後端網域不同時出問題
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Frontend", p => p
        .WithOrigins("http://localhost:3000") // ← 允許的前端來源
        .AllowAnyHeader()                    // 允許任意 Header（例如 Authorization）
        .AllowAnyMethod());                  // 允許 GET、POST、PUT、DELETE 等
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Swagger安全設定
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DocSchedule.API", Version = "v1" });

    //加入JWT Bearer設定
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    //所有API都要可以用Authorize
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//Jwt驗證邏輯設定
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var cfg = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = cfg["Issuer"],
            ValidAudience = cfg["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(1) //時間誤差1分鐘，保險用
        };

        //診斷失敗原因用
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"JWT failed: {ctx.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
    options.AddPolicy("DoctorOnly", p => p.RequireRole("Doctor"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetRequiredService<HospitalContext>();

    var plainUsers = await ctx.Users
        .Where(u => !u.PasswordHash.StartsWith("$2"))
        .ToListAsync();

    foreach (var u in plainUsers)
        u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(u.PasswordHash);

    if (plainUsers.Count > 0)
        await ctx.SaveChangesAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();    還沒有https綁定
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // 對應Controllers資料夾中的 API？

app.Run();