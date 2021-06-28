using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndAPI.Interfaces;
using BackEndAPI.Entities;
using System.Net.Http;
using System.IO;
using BackEndAPI.Helpers;
using System.Net;

namespace BackEndAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("{location}")]
        public IActionResult GetReport(int location)
        {
            var report = _reportService.GetReport(location);
            return Ok(report);
        }

        [HttpGet("ExportXls/{location}")]
        public async Task<HttpResponseMessage> ExportXls(int location)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string fileName = "";
            if(location != 0 && location != 1)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Wrong location parameter!");
            }
            if(location == 0){
                fileName = string.Concat("Report_HaNoi_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx");
            }
            if(location == 1){
                fileName = string.Concat("Report_HoChiMinh_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx");
            }
            var folderReport = "./Reports";
            string filePath = "C:/" + folderReport;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var data = _reportService.GetReport(location).ToList();
                await ReportHelper.GenerateXls(data, fullPath);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}