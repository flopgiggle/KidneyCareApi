namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.Provinces")]
    public partial class Province
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string ProvinceCode { get; set; }

        [Required]
        [StringLength(50)]
        public string ProvinceName { get; set; }
    }
}
