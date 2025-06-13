using Identity.Api.Models.DTOs;
using Identity.Api.Models.ViewModel;
using Identity.Api.Models.Views;

namespace Identity.Api.Services.Interfaces
{
    public interface IRequestService
    {
        Task<IEnumerable<RequestViewModel>> GetAllRequests();
        Task<RequestViewModel?> GetRequestById(int id);
        Task<bool> UpdateRequest(int id, RequestUpdateDTO rawRequest);
        Task<int?> CreateRequest(RequestCreateDTO rawRequest);
        Task<int?> DeleteRequest(int id);
    }
}
