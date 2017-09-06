namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.AuthenCode")]
    public partial class AuthenCode
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string PhoneNum { get; set; }

        [Column("AuthenCode")]
        [StringLength(255)]
        public string AuthenCode1 { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }
    }
}
