using Identity.Api.Models;
using Identity.Api.Models.DTOs;
using Identity.Api.Services.Interfaces;
using Identity.Api.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestsService;
        private readonly ILogger<RequestsController> _logger;

        public RequestsController(IRequestService requestsService, ILogger<RequestsController> logger)
        {
            _requestsService = requestsService;
            _logger = logger;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            _logger.LogInformation("Fetching all requests from the database.");

            var requests = await _requestsService.GetAllRequests();
            return Ok(requests); //anche se vuoto restituisco
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            _logger.LogInformation($"Fetching request with ID: {id} from the database.");
            var request = await _requestsService.GetRequestById(id);

            if (request == null)
            {
                _logger.LogWarning($"Request with ID: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Request with ID: {id} found successfully.");
            return Ok(request);
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, RequestUpdateDTO request)
        {
            _logger.LogInformation($"UPDATE request with ID: {id} in the database.");
            var result = await _requestsService.UpdateRequest(id, request);

            if (!result)
            {
                _logger.LogWarning($"FAILED update Request with ID: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"SUCCESS update Request with ID: {id}.");
            return NoContent();
        }

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostRequest(RequestCreateDTO request)
        {
            _logger.LogInformation("CREATE new request in the database.");
            var requestId = await _requestsService.CreateRequest(request);

            if (requestId == null)
            {
                _logger.LogWarning("FAILED creation Request: invalid data.");
                return BadRequest("Invalid request data provided.");
            }

            if (requestId == -1)
            {
                _logger.LogWarning($"FAILED creation Request: User with ID {request.UserId} not found.");
                return NotFound($"User with ID {request.UserId} not found.");
            }

            _logger.LogInformation($"SUCCESS creating Request with ID: {requestId}.");
            return CreatedAtAction("GetRequest", new { id = requestId }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            _logger.LogInformation($"DELETE request with ID: {id} from the database.");
            var result = await _requestsService.DeleteRequest(id);

            if (result == null)
            {
                _logger.LogWarning($"FAILED deleting Request with ID: {id}not found.");
                return NotFound();
            }

            _logger.LogInformation($"SUCCESS deleting Request with ID: {id}");
            return NoContent();
        }
    }
}
