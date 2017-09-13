namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.Cities")]
    public partial class City
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string CityCode { get; set; }

        [Required]
        [StringLength(50)]
        public string CityName { get; set; }

        [Required]
        [StringLength(20)]
        public string ProvinceCode { get; set; }
    }
}
