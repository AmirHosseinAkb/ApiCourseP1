using Entities;

namespace Services.Jwt
{
    public interface IJwtService
    {
        Task<string> Generate(User user);
    }
}
