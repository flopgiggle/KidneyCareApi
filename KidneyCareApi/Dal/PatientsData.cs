namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.PatientsData")]
    public partial class PatientsData
    {
        public int Id { get; set; }

        public int? PatientId { get; set; }

        [StringLength(255)]
        public string DataValue { get; set; }

        public int? DataCode { get; set; }

        public int? FormType { get; set; }

        [StringLength(255)]
        public string RecordTime { get; set; }

        [StringLength(255)]
        public string RecordDate { get; set; }

        public int? ReportId { get; set; }

        [StringLength(255)]
        public string Unit { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual Report Report { get; set; }
    }
}
