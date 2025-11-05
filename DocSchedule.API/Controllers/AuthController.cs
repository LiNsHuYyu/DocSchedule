using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DocSchedule.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DocSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _cfg;
    private readonly HospitalContext _db;



    public AuthController(HospitalContext db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }
    public record LoginReq(string UserName, string Password);
    public record LoginRes(int UserId, string Role, int? DoctorId, string Token);

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginRes>> Login([FromBody] LoginReq req)
    {
    
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.UserName);
        if (user == null) return Unauthorized("帳號或密碼錯誤");

        //BCrypt 驗證密碼（資料庫已存雜湊）
        bool ok = BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash);
        if (!ok) return Unauthorized("帳號或密碼錯誤");

        var token = GenerateJwt(user.Id, user.Role, user.DoctorId);
        return new LoginRes(user.Id, user.Role, user.DoctorId, token);
    }

    private string GenerateJwt(int userId, string role, int? doctorId)
    {
        var jwt = _cfg.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

        //從資料庫取名稱（Admin 用使用者帳號；Doctor 用醫師姓名，退而求其次用使用者帳號）
        var userName = _db.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Username)
            .FirstOrDefault();
        
            string displayName = userName ?? userId.ToString();
            if (role == "Doctor" && doctorId.HasValue)
            {
                var doctorName = _db.Doctors
                    .Where(d => d.Id == doctorId.Value)
                    .Select(d => d.Name)
                    .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(doctorName))
                    displayName = doctorName;
            }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, displayName),      //標準 Name，前端會讀到 name/unique_name
            new Claim("displayName", displayName),        //自訂備用欄位
        };
        if (doctorId.HasValue)
            claims.Add(new Claim("doctorId", doctorId.Value.ToString()));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpiresMinutes"]!));

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}