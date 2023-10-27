using Entities;

namespace Data.Contracts
{
    public interface IUserRepository:IRepository<User>
    {
        public Task<User> GetByUserAndPass(string username, string password,CancellationToken cancellationToken);
    }
}
