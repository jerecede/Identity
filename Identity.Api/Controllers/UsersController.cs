using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity.Api.Services.Models;
using Identity.Api.Services.Interfaces;
using Identity.Api.Models.DTOs;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("Fetching all users from the database.");

            var users = await _usersService.GetAllUsers();
            return Ok(users); //anche se vuoto restituisco
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            _logger.LogInformation($"Fetching user with ID: {id} from the database.");
            var user = await _usersService.GetUserById(id);

            if (user == null)
            {
                _logger.LogWarning($"User with ID: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"User with ID: {id} found successfully.");
            return Ok(user);
        }

        // GET: api/Users/5/roles
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles([FromRoute] int id)
        {
            _logger.LogInformation($"Fetching roles from user with ID: {id} from the database.");
            var roles = await _usersService.GetRolesFromId(id);

            if (roles == null)
            {
                _logger.LogWarning($"User with ID: {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"User with ID: {id} found successfully, returning roles.");
            return Ok(roles);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] UserUpdateDTO user)
        {
            _logger.LogInformation($"UPDATE user with ID: {id} in the database.");
            var result = await _usersService.UpdateUser(id, user);

            if(!result)
            {
                _logger.LogWarning($"FAILED update User with ID: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"SUCCESS update User with ID: {id}.");
            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserCreateDTO user)
        {
            _logger.LogInformation("CREATE new user in the database.");
            var userId = await _usersService.CreateUser(user);

            if (userId == null)
            {
                _logger.LogWarning("FAILED creation User: invalid data.");
                return BadRequest("Invalid user data provided.");
            }

            _logger.LogInformation($"SUCCESS creating User with ID: {userId}.");
            return CreatedAtAction("GetUser", new { id = userId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            _logger.LogInformation($"DELETE user with ID: {id} from the database.");
            var result = await _usersService.DeleteUser(id);

            if (result == null)
            {
                _logger.LogWarning($"FAILED deleting User with ID: {id}not found.");
                return NotFound();
            }

            _logger.LogInformation($"SUCCESS deleting User with ID: {id}");
            return NoContent();
        }
    }
}
