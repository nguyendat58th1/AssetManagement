using System;
using BackEndAPI.Enums;

namespace BackEndAPI.Models
{
    public class ReturnRequestDTO
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }

        public string AssetName { get; set; }

        public DateTime AssignedDate { get; set; }

        public string RequestedByUser { get; set; }

        public string AcceptedByUser { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public RequestState State { get; set; }
    }
}