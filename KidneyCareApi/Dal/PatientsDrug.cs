namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.PatientsDrugs")]
    public partial class PatientsDrug
    {
        public int Id { get; set; }

        public int? PatientId { get; set; }

        [StringLength(255)]
        public string DrugCode { get; set; }

        [StringLength(255)]
        public string DrugName { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }

        [StringLength(255)]
        public string RecordBatch { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
