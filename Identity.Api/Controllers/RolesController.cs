using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity.Api.Models;
using Identity.Api.Services.Models;
using Identity.Api.Services.Interfaces;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _logger = logger;
            _roleService = roleService;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            _logger.LogInformation("Fetching all users from the database.");

            var roles = await _roleService.GetAllRoles();
            return Ok(roles); //anche se vuoto restituisco
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole([FromRoute] int id)
        {
            _logger.LogInformation($"Fetching role with ID: {id} from the database.");
            var role = await _roleService.GetRoleById(id);

            if (role == null)
            {
                _logger.LogWarning($"Role with ID: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Role with ID: {id} found successfully.");
            return Ok(role);
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole([FromRoute] int id, [FromBody] string nameRole)
        {
            _logger.LogInformation($"UPDATE role with ID: {id} in the database.");
            var result = await _roleService.UpdateRole(id, nameRole);

            if (!result)
            {
                _logger.LogWarning($"FAILED update Role with ID: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"SUCCESS update Role with ID: {id}.");
            return NoContent();
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostRole([FromBody] string nameRole)
        {
            _logger.LogInformation("CREATE new role in the database.");
            var roleId = await _roleService.CreateRole(nameRole);

            if (roleId == null)
            {
                _logger.LogWarning("FAILED creation Role: invalid data.");
                return BadRequest("Invalid role data provided.");
            }

            _logger.LogInformation($"SUCCESS creating Role with ID: {roleId}.");
            return CreatedAtAction("GetRole", new { id = roleId }, nameRole);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
            _logger.LogInformation($"DELETE role with ID: {id} from the database.");
            var result = await _roleService.DeleteRole(id);

            if (result == null)
            {
                _logger.LogWarning($"FAILED deleting Role with ID: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"SUCCESS deleting Role with ID: {id}");
            return NoContent();
        }
    }
}
