using System;
using System.Collections.Generic;
using BackEndAPI.Enums;

namespace BackEndAPI.Entities
{
    public class CreateAssetModel
    {

        public string AssetName { get; set; }

        public int CategoryId { get; set; }

        public string Specification { get; set; }

        public DateTime? InstalledDate { get; set; }

        public AssetState State { get; set; }

        public Location Location { get; set; }

    }
}