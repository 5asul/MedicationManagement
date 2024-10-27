using MedicationManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text;

namespace MedicationManagement.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        private readonly string _issuer = configuration["Jwt:Issuer"]!;
        private readonly string _audience = configuration["Jwt:Audience"]!;
        private readonly string _secretKey = configuration["Jwt:Key"]!;

        public string GenerateToken(string name, string email, string role)
        {
            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),// إضافة اسم المستخدم كادعاء (Claim)
                Expires = DateTime.UtcNow.AddHours(1), // تحديد مدة صلاحية التوكن
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            String token = tokenHandler.CreateToken(tokenDescriptor);
            return token; // إعادة التوكن كـ سلسلة نصية
        }
    }
}
