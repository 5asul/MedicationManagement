using MedicationManagement.Data;
using MedicationManagement.Models;
using MedicationManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

namespace MedicationManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(ApplicationDbContext context, ITokenService itokenService) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ITokenService _tokenService = itokenService;

        [HttpPost("register")]
        public IActionResult Register(String Role, String FullName, String Email, String Password, String PhoneNumber, DateTime DateOfBirth)
        {
            var user = new User
            {
                
                Role = Role,
                FullName = FullName,
                Email = Email,
                Password = Password,
                PhoneNumber = PhoneNumber,
                DateOfBirth = DateOfBirth,


            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password );
            if (user == null)
                return Unauthorized();

            
            var token = _tokenService.GenerateToken(user.FullName, user.Email, user.Role);
            return Ok(new { token });
        }

        
    }

}
