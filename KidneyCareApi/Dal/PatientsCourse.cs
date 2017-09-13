namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.PatientsCourse")]
    public partial class PatientsCourse
    {
        public int Id { get; set; }

        public int? PaitentId { get; set; }

        [StringLength(255)]
        public string CoursCode { get; set; }

        [StringLength(255)]
        public string CoursName { get; set; }

        public int? CoursStatus { get; set; }

        [StringLength(255)]
        public string AttendingDates { get; set; }

        public int? ObjectCode { get; set; }

        [StringLength(255)]
        public string ObjectName { get; set; }

        public int? ModeCode { get; set; }

        [StringLength(255)]
        public string ModeName { get; set; }

        public int? CognitionCode { get; set; }

        [StringLength(255)]
        public string CognitionName { get; set; }

        public int? BehaviorCode { get; set; }

        [StringLength(255)]
        public string BehaviorName { get; set; }

        [StringLength(255)]
        public string Mark { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
