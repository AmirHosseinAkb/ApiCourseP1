using Entities;

namespace Services.Jwt
{
    public interface IJwtService
    {
        string Generate(User user);
    }
}
