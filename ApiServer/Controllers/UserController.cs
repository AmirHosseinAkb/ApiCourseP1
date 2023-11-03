using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Jwt;
using WebFramework.Api;
using WebFramework.DTOs;
using WebFramework.Filters;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiResultFilter]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public UserController(IUserRepository userRepository,IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            return BadRequest();
            return await _userRepository.TableNoTracking.ToListAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id,CancellationToken cancellationToken)
        {
           var user=await _userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("[action]")]
        public async Task<string> GenerateToken(string userName, string password, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserAndPass(userName, password, cancellationToken);
            if (user is null)
                throw new BadRequestException();
            var jwt = _jwtService.Generate(user);
            return jwt;
        }


        [HttpPost]
        public async Task<ActionResult> Create(UserDto user,CancellationToken cancellationToken)
        {
            var newUser = new User()
            {
                Age = user.Age,
                FullName = user.FullName,
                Gender = user.Gender,
                PasswordHash = SecurityHelper.HashPasswordSHA256(user.Password),
                UserName = user.UserName,
            };
            await _userRepository.AddAsync(newUser, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<ApiResult> Update(int id, User user,CancellationToken cancellationToken)
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
            throw new Exception();
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            if(user != null)
            {
               await _userRepository.DeleteAsync(user, cancellationToken);
                return Ok();
            }
            return NotFound();
        }
    }
}
