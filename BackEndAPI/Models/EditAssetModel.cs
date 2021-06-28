using System;
using System.ComponentModel.DataAnnotations;
using BackEndAPI.Entities;
using BackEndAPI.Enums;

namespace BackEndAPI.Models
{
    public class EditAssetModel
    {
        [Required]
        public string AssetName { get; set; }

        [Required]
        public string Specification { get; set; }

        [Required]

        public DateTime InstalledDate { get; set; }

        [Required]
        public AssetState State { get; set; }
    }
}