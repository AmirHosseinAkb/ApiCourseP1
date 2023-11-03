using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities;
using Microsoft.IdentityModel.Tokens;

namespace Services.Jwt
{
    public class JwtService:IJwtService
    {
        public async Task<string> Generate(User user)
        {
            var secretKey = "MySecretKeyForMyWebsite";
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                , SecurityAlgorithms.HmacSha256Signature);
            var claims = _getClaims(user);
            var descriptor = new SecurityTokenDescriptor()
            {
                Issuer = "MyWebsite",
                Audience = "MyWebsite",
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddDays(14),
                SigningCredentials = signingCredentials,
                IssuedAt = DateTime.Now,
                Subject = new ClaimsIdentity(claims)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var jwt = tokenHandler.WriteToken(securityToken);
            return jwt;
        }

        private IEnumerable<Claim> _getClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.MobilePhone, "09222222222")
            };

            var roles = new Role[]
            {
                new Role() {Name = "Admin"},
                new Role() {Name = "Assistant"}
            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role,role.Name));
            return claims;
        }
    }
}
