namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.CourseTakeIn")]
    public partial class CourseTakeIn
    {
        public int Id { get; set; }

        public int? CourseId { get; set; }

        public int? UserId { get; set; }

        public int? Type { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int? CreatePerson { get; set; }

        public int? UpdatePerson { get; set; }

        public virtual Course Course { get; set; }

        public virtual User User { get; set; }
    }
}
