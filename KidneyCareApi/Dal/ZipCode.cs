namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.ZipCode")]
    public partial class ZipCode
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string AreaCode { get; set; }

        [Required]
        [StringLength(20)]
        public string ZipName { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }
    }
}
