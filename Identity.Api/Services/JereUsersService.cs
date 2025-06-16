using Identity.Api.Models.DTOs;
using Identity.Api.Models.ViewModel;
using Identity.Api.Models.Views;
using Identity.Api.Services.Interfaces;
using Identity.Api.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Identity.Api.Services
{
    public class JereUsersService : IUsersService
    {
        private readonly IdentityContext _context;

        public JereUsersService(IdentityContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userViews = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password
            });

            return userViews;
        }

        public async Task<UserViewModel?> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            var userView = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password
            };
            return userView;
        }

        public async Task<IEnumerable<RoleViewModel>?> GetRolesFromId(int id)
        {
            var user = await _context.Users //user con tutto il suo contesto (userRoles, roles
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            var roles = user.UserRoles.Select(ur => new RoleViewModel
            {
                Id = ur.Role.Id,
                Name = ur.Role.Name
            }).ToList();

            //var user = await _context.Users.FindAsync(id);

            //var userRoles = await _context.UserRoles
            //    .Where(ur => ur.UserId == user.Id)
            //    .Select(ur => ur.Role)
            //    .ToListAsync();

            //var roles = await _context.Roles
            //    .Where(role => userRoles.Any(ur => ur.Id == role.Id))
            //    .Select(role => new RoleViewModel
            //    {
            //        Id = role.Id,
            //        Name = role.Name
            //    })
            //    .ToListAsync();

            return roles;
        }

        public async Task<bool> UpdateUser(int id, UserUpdateDTO rawUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            //non mi faccio problemi se c'è invalid data, semplicemente non lo aggiorno
            if (!string.IsNullOrWhiteSpace(rawUser.FirstName) && rawUser.FirstName.Length <= 50) user.FirstName = rawUser.FirstName;
            if (!string.IsNullOrWhiteSpace(rawUser.LastName) && rawUser.LastName.Length <= 50) user.LastName = rawUser.LastName;
            if (!string.IsNullOrWhiteSpace(rawUser.Email) && rawUser.Email.Length <= 100) user.Email = rawUser.Email;
            if (!string.IsNullOrWhiteSpace(rawUser.Password) && rawUser.Password.Length <= 100) user.Password = rawUser.Password;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> CreateUser(UserCreateDTO rawUser)
        {
            if (!string.IsNullOrWhiteSpace(rawUser.FirstName) && rawUser.FirstName.Length > 50) return null;
            if (!string.IsNullOrWhiteSpace(rawUser.LastName) && rawUser.LastName.Length > 50) return null;
            if (!string.IsNullOrWhiteSpace(rawUser.Email) && rawUser.Email.Length > 100) return null;
            if (!string.IsNullOrWhiteSpace(rawUser.Password) && rawUser.Password.Length > 100) return null;

            var user = new User
            {
                FirstName = rawUser.FirstName,
                LastName = rawUser.LastName,
                Email = rawUser.Email,
                Password = rawUser.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<int?> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
