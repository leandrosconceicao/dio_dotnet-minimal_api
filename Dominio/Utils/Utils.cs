using Microsoft.IdentityModel.Tokens;
using minimal_api.Dominio.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace minimal_api.Dominio.Utils
{
    public class Utils
    {
        public static string GerarTokenJwt(Administradores administrador, string key)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>() { 
                new Claim("Email", administrador.Email),
                new Claim("Perfil", administrador.Perfil),
                new Claim(ClaimTypes.Role, administrador.Perfil)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
