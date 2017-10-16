namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.Course")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            CourseTakeIns = new HashSet<CourseTakeIn>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string CourseName { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string Date { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(255)]
        public string Speaker { get; set; }

        [StringLength(255)]
        public string SpeakerId { get; set; }

        [StringLength(1000)]
        public string SpeakerInfo { get; set; }

        [StringLength(1000)]
        public string CourseContent { get; set; }

        public int? Type { get; set; }

        [StringLength(500)]
        public string PicUrl { get; set; }

        [StringLength(500)]
        public string PPTUrl { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int? CreatePerson { get; set; }

        public int? UpdatePerson { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseTakeIn> CourseTakeIns { get; set; }
    }
}
