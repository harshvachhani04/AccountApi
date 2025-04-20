using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccountApi.Data
{
    public class JWTTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JWTTokenGenerator(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string GenerateToken(int userId, string role)
        {
            var jwtSetting = _configuration.GetSection("JWTSettings");
            var credentials= new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting["Key"])), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSetting["Issuer"],
                audience: jwtSetting["Audience"],
                expires: DateTime.Now.AddHours(Convert.ToDouble(jwtSetting[""])),
                signingCredentials: credentials,
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
