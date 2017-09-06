namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.table1")]
    public partial class table1
    {
        [Key]
        [StringLength(255)]
        public string test { get; set; }
    }
}
