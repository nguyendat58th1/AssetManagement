using System;
using System.Collections.Generic;
using BackEndAPI.Enums;

namespace BackEndAPI.Entities
{
    public class Asset : IEntity
    {
        public int Id { get; set; }

        public string AssetCode { get; set; }

        public string AssetName { get; set; }

        public int CategoryId { get; set; }
        public virtual AssetCategory Category { get; set; }

        public string Specification { get; set; }

        public DateTime? InstalledDate { get; set; }

        public AssetState State { get; set; }

        public Location Location { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

    }
}