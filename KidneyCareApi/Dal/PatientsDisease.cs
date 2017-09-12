namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.PatientsDisease")]
    public partial class PatientsDisease
    {
        public int Id { get; set; }

        public int? PatientId { get; set; }

        public int? DiseaseType { get; set; }

        public int? DiseaseCode { get; set; }

        [StringLength(255)]
        public string DiseaseName { get; set; }

        public int? DiseaseStatus { get; set; }

        [StringLength(255)]
        public string DiseaseStartTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
