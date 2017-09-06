namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.table2")]
    public partial class table2
    {
        [Key]
        public int yy { get; set; }

        [StringLength(255)]
        public string dd { get; set; }
    }
}
