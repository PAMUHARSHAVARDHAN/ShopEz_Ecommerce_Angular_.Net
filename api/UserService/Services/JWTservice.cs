using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Models;

namespace UserService.Services
{
    public class JWTservice
    {
        private readonly IConfiguration _config;
        public JWTservice(IConfiguration config)
        {
            _config = config;
        }
        public virtual string GenerateToken(User user)
        {
            // 🔹 Claims (data inside token)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            // 🔹 Secret Key
            var secret = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new InvalidOperationException("Configuration value 'Jwt:Key' is missing or empty.");
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 🔹 Create Token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            // 🔹 Return token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
