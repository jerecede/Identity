using Identity.Api.Models;
using Identity.Api.Models.ViewModel;
using Identity.Api.Services.Interfaces;
using Identity.Api.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Services
{
    public class JereRoleService : IRoleService
    {
        private readonly IdentityContext _context;

        public JereRoleService(IdentityContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            var rolesView =  roles.Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            });

            return rolesView;
        }

        public async Task<RoleViewModel?> GetRoleById(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null) return null;

            var roleView = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            return roleView;
        }

        public async Task<bool> UpdateRole(int id, string nameRole)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null) return false;

            if (!string.IsNullOrWhiteSpace(nameRole) && nameRole.Length <= 50) role.Name = nameRole;
            //me ne frego se c'è invalid data, semplicemente non lo aggiorno

            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> CreateRole(string nameRole)
        {
            if(string.IsNullOrWhiteSpace(nameRole) || nameRole.Length > 50) return null;

            var role = new Role
            {
                Name = nameRole
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role.Id;
        }

        public async Task<int?> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            //c'è gia cascade quindi non mi faccio problemi con le foreign key

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return role.Id;
        }
    }
}
