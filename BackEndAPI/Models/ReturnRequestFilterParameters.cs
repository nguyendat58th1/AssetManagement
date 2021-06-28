using System;
using BackEndAPI.Enums;

namespace BackEndAPI.Models
{
    public class ReturnRequestFilterParameters
    {

        public DateTime? ReturnedDate { get; set; }

        public RequestState? RequestState { get; set; }
    }
}
