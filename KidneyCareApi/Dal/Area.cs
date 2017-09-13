namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.Areas")]
    public partial class Area
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string AreaCode { get; set; }

        [Required]
        [StringLength(50)]
        public string AreaName { get; set; }

        [Required]
        [StringLength(20)]
        public string CityCode { get; set; }
    }
}
