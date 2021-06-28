using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndAPI.Interfaces;
using BackEndAPI.Entities;

namespace Namespace
{
    // [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetCategoriesController : ControllerBase
    {

        private readonly IAssetCategoryService _service;

        public AssetCategoriesController(IAssetCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IQueryable<AssetCategory>> Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryModel model)
        {
            return Ok(await _service.Create(model));
        }

        [HttpGet("checkName/{categoryName}")]
        public bool CheckCategoryName(string categoryName)
        {
            return _service.DidCategoryNameExist(categoryName);
        }

        [HttpGet("checkCode/{categoryCode}")]
        public bool CheckCategoryCode(string categoryCode)
        {
            return _service.DidCategoryCodeExist(categoryCode);
        }

    }
}