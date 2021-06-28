using BackEndAPI.Entities;

namespace BackEndAPI.Models
{
    public class ReportModel
    {
        public int ID { get; set;}
        public string CategoryName { get; set;}
        public int Total { get; set;}
        public int Assigned { get; set;}
        public int Available { get; set;}
        public int NotAvailable { get; set;}
        public int WaitingForRecycling { get; set;}
        public int Recycled { get; set;}
    }
}