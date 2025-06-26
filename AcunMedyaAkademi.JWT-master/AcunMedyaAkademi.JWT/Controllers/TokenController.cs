using AcunMedyaAkademi.JWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AcunMedyaAkademi.JWT.Controllers
{
    public class TokenController : Controller
    {
        private readonly JwtSettingsModel _jwtSettings;

        public TokenController(IOptions<JwtSettingsModel> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        [HttpGet]
        public IActionResult CreateToken()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateToken(UserİnfoDto dto)
        {

            var claims = new[]
            {
                new Claim("name",dto.Name),
                new Claim("surname",dto.Surname),
                new Claim("role",dto.Role),
                new Claim("city",dto.City),

                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                signingCredentials: creds
                );
            dto.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return View(dto);
        }
    }
}
