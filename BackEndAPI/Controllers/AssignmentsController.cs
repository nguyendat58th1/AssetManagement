using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Helpers;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using BackEndAPI.Models;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPost("{userId}")]
        public async Task<IActionResult> Post(int userId, AssignmentModel model)
        {
            await _assignmentService.Create(userId, model);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AssignmentModel model)
        {
            await _assignmentService.Update(id, model);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("getAllNoCondition")]
        public IQueryable<Assignment> GetAllNoCondition()
        {
            return _assignmentService.GetAll();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getAllForEachUser/{userId}")]
        public async Task<IQueryable<Assignment>> GetAllForEachUser(int userId)
        {

            return await _assignmentService.GetAllForUser(userId);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAllAssignments(
             [FromQuery] PaginationParameters paginationParameters
            )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assignments = await _assignmentService.GetAllAssignments(paginationParameters, Int32.Parse(adminClaim.Value));

            return Ok(assignments);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<Assignment> GetById(int id)
        {
            var assignment = await _assignmentService.GetById(id);
            return assignment;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("state/{state}")]
        public async Task<ActionResult<GetAssignmentListPagedResponse>> GetAssignmetByType(
         AssignmentState state,
         [FromQuery] PaginationParameters paginationParameters
     )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assignment = await _assignmentService.GetAssignmentByState(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                state
            );

            return Ok(assignment);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("date/{month}/{day}/{year}")]
        public async Task<ActionResult<GetAssignmentListPagedResponse>> GetAssignmetByDate(
         int year, int month, int day,
         [FromQuery] PaginationParameters paginationParameters
     )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assignment = await _assignmentService.GetAssignmentByDate(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                year,
                month,
                day
            );

            return Ok(assignment);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("search")]
        public async Task<ActionResult<GetAssignmentListPagedResponse>> SearchAssignments(
            [FromQuery] string query,
            [FromQuery] PaginationParameters paginationParameters
        )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assignment = await _assignmentService.SearchAssignments(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                query
            );

            return Ok(assignment);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _assignmentService.Delete(id);
            return Ok();
        }
        
        [HttpPut("accept/{id}")]
        public async Task<IActionResult> AcceptAssignment(int id)
        {
            await _assignmentService.AcceptAssignment(id);
            return Ok(new {message = Message.AssignmentAccepted});
        }

        [HttpPut("decline/{id}")]
        public async Task<IActionResult> DeclineAssignment(int id)
        {
            await _assignmentService.DeclineAssignment(id);
            return Ok(new {message = Message.AssignmentDeclined});
        }
        
        [HttpPut("undo-respone/{id}")]
        public async Task<IActionResult> UndoResponeAssignment(int id)
        {
            await _assignmentService.UndoResponeAssignment(id);
            return Ok(new {message = Message.AssignmentUndidRespone});
        }
    }
}