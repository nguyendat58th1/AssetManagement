using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using BackEndAPI.Enums;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace BackEndAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly IAsyncAssetRepository _assetRepository;
        private readonly IAsyncAssetCategoryRepository _categoryRepository;
        public IConfiguration _configuration { get; }
        public ReportService(IAsyncAssetRepository assetRepository, IAsyncAssetCategoryRepository categoryRepository, IConfiguration configuration)
        {
            _assetRepository = assetRepository;
            _categoryRepository = categoryRepository;
            _configuration = configuration;
        }

    public IEnumerable<ReportModel> GetReport(int location)
    {
        IList<ReportModel> reportList = new List<ReportModel>();
        try{
            var con= _configuration.GetConnectionString("SqlConnection");
            using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    SqlParameter PmtrLocation = new SqlParameter();  
                    PmtrLocation.ParameterName = "@lo"; 
                    PmtrLocation.SqlDbType = SqlDbType.Int;
                    PmtrLocation.Direction = ParameterDirection.Input;
                    PmtrLocation.Value = location;

                    string sql = "SELECT CategoryName"
                                +        ",Total = (SELECT COUNT(A.Id) " 
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.Location = @lo)"
                                +        ",Assigned = (SELECT COUNT(A.Id)" 
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 2 AND A.Location = @lo)"
                                +        ",Available = (SELECT COUNT(A.Id) "
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 0 AND A.Location = @lo)"
                                +        ",NotAvailable = (SELECT COUNT(A.Id) "
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 1 AND A.Location = @lo)"
                                +        ",WaitingForRecycling = (SELECT COUNT(A.Id) "
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 3 AND A.Location = @lo)"
                                +        ",Recycled = (SELECT COUNT(A.Id) "
                                +                    "FROM [Assets] A "
                                +                    "WHERE A.CategoryId = AC.Id AND A.State = 4 AND A.Location = @lo)"
                                +"FROM AssetCategories AC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add(PmtrLocation); 
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int count = 1;
                            while (reader.Read())
                            {
                                reportList.Add(new ReportModel
                                {
                                    ID = count,
                                    CategoryName = reader["CategoryName"].ToString(),
                                    Total = Int32.Parse(reader["Total"].ToString()),
                                    Assigned = Int32.Parse(reader["Assigned"].ToString()),
                                    Available = Int32.Parse(reader["Available"].ToString()),
                                    NotAvailable = Int32.Parse(reader["NotAvailable"].ToString()),
                                    WaitingForRecycling = Int32.Parse(reader["WaitingForRecycling"].ToString()),
                                    Recycled = Int32.Parse(reader["Recycled"].ToString()),
                                });
                                count++;
                            }
                        }
                    }                    
                }
        }catch (SqlException ex)
        {
            throw ex;
        }

        return reportList;
    }
  }
}