using Identity.Api.Models.DTOs;
using Identity.Api.Models.ViewModel;

namespace Identity.Api.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleViewModel>> GetAllRoles();
        Task<RoleViewModel?> GetRoleById(int id);
        Task<bool> UpdateRole(int id, string nameRole);
        Task<int?> CreateRole(string nameRole);
        Task<int?> DeleteRole(int id);
    }
}
