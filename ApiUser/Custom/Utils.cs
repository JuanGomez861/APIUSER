using Dominio.Entidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiUser.Custom
{
    public class Utils
    {
        private readonly IConfiguration _configuration;

        public Utils(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public string generarJwt(Cuenta cuenta)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Email,cuenta.Correo),
                new Claim(ClaimTypes.Role,cuenta.Rol),


            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);


        }
    }
}
