namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.Message")]
    public partial class Message
    {
        public int Id { get; set; }

        public int? ToUser { get; set; }

        public int? FromUser { get; set; }

        [StringLength(1000)]
        public string Messge { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
