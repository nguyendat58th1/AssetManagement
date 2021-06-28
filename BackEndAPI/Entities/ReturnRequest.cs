using System;
using BackEndAPI.Enums;

namespace BackEndAPI.Entities
{
    public class ReturnRequest : IEntity
    {
        public int Id { get; set; }

        public int? AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }

        public string AssetCodeCopy { get; set; }
        
        public string AssetNameCopy { get; set; }

        public DateTime AssignedDateCopy {get; set;}

        public int RequestedByUserId { get; set; }

        public virtual User RequestedByUser { get; set; }

        public int? AcceptedByUserId { get; set; }

        public virtual User AcceptedByUser { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public RequestState State { get; set; }

    }
}