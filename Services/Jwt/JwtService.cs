using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common;
using Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Services.Jwt
{
    public class JwtService:IJwtService
    {
        private readonly SiteSettings _siteSettings;

        public JwtService(IOptionsSnapshot<SiteSettings> siteSettings)
        {
            _siteSettings = siteSettings.Value;
        }
        public string Generate(User user)
        {
            var secretKey = _siteSettings.JwtSettings.SecretKey;
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                , SecurityAlgorithms.HmacSha256Signature);
            var claims = _getClaims(user);
            var descriptor = new SecurityTokenDescriptor()
            {
                Issuer = _siteSettings.JwtSettings.Issuer,
                Audience = _siteSettings.JwtSettings.Audience,
                NotBefore = DateTime.Now.AddMinutes(_siteSettings.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddDays(_siteSettings.JwtSettings.ExpireDays),
                SigningCredentials = signingCredentials,
                IssuedAt = DateTime.Now,
                Subject = new ClaimsIdentity(claims)
            };
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
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
