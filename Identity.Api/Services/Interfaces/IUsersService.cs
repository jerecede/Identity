using Identity.Api.Models.DTOs;
using Identity.Api.Models.ViewModel;
using Identity.Api.Models.Views;
using Identity.Api.Services.Models;

namespace Identity.Api.Services.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsers();
        Task<UserViewModel?> GetUserById(int id);
        Task<IEnumerable<RoleViewModel>?> GetRolesFromId(int id);
        Task<bool> UpdateUser(int id, UserUpdateDTO chirp);
        Task<int?> CreateUser(UserCreateDTO chirp);
        Task<int?> DeleteUser(int id);
    }
}
