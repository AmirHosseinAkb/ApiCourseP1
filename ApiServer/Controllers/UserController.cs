using Data.Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public Task<List<User>> Get(CancellationToken cancellationToken)
        {
            return _userRepository.TableNoTracking.ToListAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public Task<User> Get(int id,CancellationToken cancellationToken)
        {
            return _userRepository.GetByIdAsync(cancellationToken, id);
        }

        [HttpPost]
        public async Task Create(User user,CancellationToken cancellationToken)
        {
            await _userRepository.AddAsync(user, cancellationToken);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, User user,CancellationToken cancellationToken)
        {
            var currUser =await _userRepository.GetByIdAsync(cancellationToken,id);
            if(currUser != null)
            {
                currUser.UserName = user.UserName;
                currUser.Gender = user.Gender;
                currUser.Age= user.Age;
                currUser.PasswordHash = user.PasswordHash;
                currUser.LastLoginDate = user.LastLoginDate;
                currUser.FullName = user.FullName;
                await _userRepository.UpdateAsync(currUser, cancellationToken);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            if(user != null)
            {
               await _userRepository.DeleteAsync(user, cancellationToken);
                return Ok();
            }
            return BadRequest();
        }
    }
}
