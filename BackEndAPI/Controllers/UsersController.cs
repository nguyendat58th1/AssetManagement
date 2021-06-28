using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackEndAPI.Enums;
using BackEndAPI.Helpers;
using System.Linq;
using BackEndAPI.Entities;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("{id}")]
        public async Task<UserInfo> Get(int id)
        {
            return await _userService.GetById(id);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserModel user)
        {
            return Ok(await _userService.Create(user));
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EditUserModel model)
        {
            await _userService.Update(id, model);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPut("{userId}/disable/{id}")]
        public async Task<IActionResult> Disabled(int userId, int id)
        {
            await _userService.Disable(userId, id);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("getalluser/{userId}")]
        public async Task<IQueryable<UserDTO>> GetAll(int userId)
        {
            return await _userService.GetAllUsers(userId);

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getAllNoCondition")]
        public IEnumerable<User> GetAllNoCondition()
        {
            return _userService.GetAll();

        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("search/{userId}/{searchText}")]
        public async Task<IQueryable<UserDTO>> GetUserBySearching(int userId, string searchText)
        {
            return await _userService.GetUserBySearching(userId, searchText);

        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> GetAllUsers(
                    [FromQuery] PaginationParameters paginationParameters
                )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _userService.GetUsers(paginationParameters, Int32.Parse(adminClaim.Value));

            return Ok(users);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("params")]
        public async Task<ActionResult<GetUsersListPagedResponseDTO>> GetUsersWithParams(
                    [FromQuery] UserSearchFilterParameters searchFilterParameters,
                    [FromQuery] UserSortParameters sortParameters,
                    [FromQuery] PaginationParameters paginationParameters
                )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _userService.GetUsers(
                Int32.Parse(adminClaim.Value),
                searchFilterParameters,
                sortParameters,
                paginationParameters
            );

            return Ok(users);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest model)
        {
            var user = _userService.GetUserByIdWithPassword(id);
            if (user == null)
            {
                return NotFound(Message.UserNotFound);
            }
            if (user.Result.OnFirstLogin == OnFirstLogin.No)
            {
                if (user.Result.Password != model.OldPassword)
                {
                    return BadRequest(new { message = Message.OldPasswordIncorrect });
                }
            }
            await _userService.ChangePassword(id, model);
            return Ok(new { message = Message.ChangePasswordSucceed });
        }
    }
}