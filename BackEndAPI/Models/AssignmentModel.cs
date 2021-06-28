using System;

namespace BackEndAPI.Models
{
    public class AssignmentModel
    {
        public int AssetId { get; set;}
        public int AssignedToUserId { get; set;}

        public DateTime AssignedDate { get; set; }

         public string Note { get; set; }

    }
}