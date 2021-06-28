using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndAPI.Interfaces;
using BackEndAPI.Entities;
using BackEndAPI.Models;
using System.Security.Claims;
using BackEndAPI.Enums;

namespace Namespace
{
     // [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {

        private readonly IAssetService _service;

        public AssetsController(IAssetService service)
        {
            _service = service;
        }
        
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<GetAssetsListPagedResponseDTO>> GetAllAssets(
                    [FromQuery] PaginationParameters paginationParameters
                )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _service.GetAssets(paginationParameters, Int32.Parse(adminClaim.Value));

            return Ok(users);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("{id}")]
        public async Task<Asset> Get(int id)
        {
            return await _service.GetById(id);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<GetAssetsListPagedResponseDTO>> GetAssetsByCategory(
                    int categoryId,
                    [FromQuery] PaginationParameters paginationParameters
                )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assets = await _service.GetAssetsByCategory(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                categoryId
            );

            return Ok(assets);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("state/{state}")]
        public async Task<ActionResult<GetAssetsListPagedResponseDTO>> GetAssetsByState(
                    AssetState state,
                    [FromQuery] PaginationParameters paginationParameters
                )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var assets = await _service.GetAssetsByState(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                state
            );

            return Ok(assets);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("search")]
        public async Task<ActionResult<GetAssetsListPagedResponseDTO>> SearchAssets(
                    [FromQuery] string query,
                    [FromQuery] PaginationParameters paginationParameters
                )
        {
            var adminClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            var users = await _service.SearchAssets(
                paginationParameters,
                Int32.Parse(adminClaim.Value),
                query
            );

            return Ok(users);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAssetModel model)
        {
            return Ok(await _service.Create(model));
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("getallasset/{userId}")]
        public async Task<IQueryable<Asset>> GetAll(int userId)
        {
            return await _service.GetAllAssets(userId);

        }


        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpGet("search/{userId}/{searchText}")]
        public async Task<IQueryable<Asset>> GetUserBySearching(int userId, string searchText)
        {
            return await _service.GetAssetsBySearching(userId, searchText);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EditAssetModel model)
        {
            await _service.Update(id, model);
            return Ok();
        }
        
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok();
        }

    }
}