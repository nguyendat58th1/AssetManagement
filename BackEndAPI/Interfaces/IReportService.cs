using System.Collections.Generic;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IReportService
    {
        
        IEnumerable<ReportModel> GetReport(int location);
        
    }
}