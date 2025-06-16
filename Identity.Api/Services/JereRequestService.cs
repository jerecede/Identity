using Identity.Api.Models;
using Identity.Api.Models.DTOs;
using Identity.Api.Models.ViewModel;
using Identity.Api.Services.Interfaces;
using Identity.Api.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Services
{
    public class JereRequestService : IRequestService
    {
        private readonly IdentityContext _context;

        public JereRequestService(IdentityContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RequestViewModel>> GetAllRequests()
        {
            var requests = await _context.Requests.ToListAsync();

            var requestViews = requests.Select(request => new RequestViewModel
            {
                Id = request.Id,
                UserId = request.UserId,
                Text = request.Text,
                CreationTime = request.CreationTime
            });

            return requestViews;
        }

        public async Task<RequestViewModel?> GetRequestById(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null) return null;
            return new RequestViewModel
            {
                Id = request.Id,
                UserId = request.UserId,
                Text = request.Text,
                CreationTime = request.CreationTime
            };
        }

        public async Task<bool> UpdateRequest(int id, RequestUpdateDTO rawRequest)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null) return false;

            if(!string.IsNullOrWhiteSpace(rawRequest.Text) && rawRequest.Text.Length <= 500) request.Text = rawRequest.Text;
            //me ne frego se c'è invalid data, semplicemente non lo aggiorno

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> CreateRequest(RequestCreateDTO rawRequest)
        {
            if (!string.IsNullOrWhiteSpace(rawRequest.Text) && rawRequest.Text.Length > 500) return null;

            var user = await _context.Users.FindAsync(rawRequest.UserId);
            if (rawRequest.UserId <= 0 || user == null) return -1;

            var request = new Request
            {
                UserId = rawRequest.UserId,
                Text = rawRequest.Text
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return request.Id;
        }

        public async Task<int?> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null) return null;

            //c'è gia cascade quindi non mi faccio problemi con le foreign key

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return request.Id;
        }
    }
}
