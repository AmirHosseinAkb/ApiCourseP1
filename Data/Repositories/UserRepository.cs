using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken)
        {
            var hashedPassword = SecurityHelper.HashPasswordSHA256(password);
            return await Table
                .SingleOrDefaultAsync(u => u.UserName == username && u.PasswordHash == hashedPassword, cancellationToken);
        }

        public override async Task AddAsync(User entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            if (await IsExistAsync(u => u.UserName == entity.UserName, cancellationToken))
                throw new BadRequestException("This User Name Exist In Advance");
            await base.AddAsync(entity, cancellationToken, saveNow);
        }
    }
}
