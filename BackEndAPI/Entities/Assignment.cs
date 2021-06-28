using System;
using BackEndAPI.Enums;

namespace BackEndAPI.Entities
{
    public class Assignment : IEntity
    {
        public int Id { get; set; }

        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public int AssignedByUserId { get; set; }
        public virtual User AssignedByUser { get; set; }

        public string AssignedToUserName { get; set; }

        public int AssignedToUserId { get; set; }
        public virtual User AssignedToUser { get; set; }

        public DateTime AssignedDate { get; set; }

        public AssignmentState State { get; set; }

        public string Note { get; set; }

        public DateTime CreateEditDate { get; set;}

        public virtual ReturnRequest Request { get; set; }
        
    }
}