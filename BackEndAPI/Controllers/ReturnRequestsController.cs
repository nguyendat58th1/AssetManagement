using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndAPI.Models;
using System.Security.Claims;
using System;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReturnRequestsController : ControllerBase
    {
        private readonly IReturnRequestService _service;

        public ReturnRequestsController(IReturnRequestService service)
        {
            _service = service;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<GetReturnRequestsPagedResponseDTO>> GetAll(
                    [FromQuery] PaginationParameters paginationParameters
                )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var returnRequests = await _service.GetAll(paginationParameters, Int32.Parse(adminClaim.Value));            

            return Ok(returnRequests);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("filter")]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> Filter(
                    [FromQuery] ReturnRequestFilterParameters filterParameters,
                    [FromQuery] PaginationParameters paginationParameters
                )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _service.Filter(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                filterParameters
            );

            return Ok(users);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("search")]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> Search(
                    [FromQuery] string query,
                    [FromQuery] PaginationParameters paginationParameters
                )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _service.Search(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                query
            );

            return Ok(users);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateReturnRequestModel model)
        {
            var userClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var userId = Int32.Parse(userClaim.Value); 
            return Ok(await _service.Create(model, userId));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("count")]
        public ActionResult<int> GetAssociatedActiveCount([FromQuery] string assetCode)
        {
            var userClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            return Ok(_service.GetAssociatedActiveCount(assetCode));
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPut("{rrId}/approve")]
        public async Task<IActionResult> Approve(int rrId) {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);

            await _service.Approve(rrId, Int32.Parse(adminClaim.Value));
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPut("{rrId}/deny")]
        public async Task<IActionResult> Deny(int rrId) {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);

            await _service.Deny(rrId, Int32.Parse(adminClaim.Value));
            return Ok();
        }
    }
}

