using Dapper;
using Domain.Common;
using Domain.Entities.Users;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(int id);
    }

    [Route("v1/users")]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IUserService _userService;

        public UsersController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUsers([FromBody] UserRequest model)
        {
            var user = new User();
            user.username = model.username;
            user.setPassword(model.password);

            await _userRepository.AddAsync(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();

            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUsers([FromRoute] int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) {
                throw new NotFoundRuleException("not_found", "not_found");
            }

            await _userRepository.DeleteAsync(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();

            return Ok(user);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditUsers([FromRoute] int id, [FromBody] UserRequest model)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) {
                throw new NotFoundRuleException("not_found", "not_found");
            }
            user.username = model.username;
            user.setPassword(model.password);

            await _userRepository.UnitOfWork.SaveEntitiesAsync();
            return Ok(user);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUsers([FromRoute] int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) {
                throw new NotFoundRuleException("not_found", "not_found");
            }

            return Ok(user);
        }

        public class UserRequest
        {
            public string password { get; set; }
            public string username { get; set; }
        }
    }

    public class UserService : IUserService
    {
        private readonly ISqlConnectionFactory _factory;

        public UserService(ISqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            const string commandText = "select * from Users where id = @id";

            using var conn = _factory.GetOpenConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(commandText, new {
                id
            });
        }
    }
}