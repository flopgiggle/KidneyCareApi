namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.IndicatorsRange")]
    public partial class IndicatorsRange
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Min { get; set; }

        [StringLength(255)]
        public string Max { get; set; }

        [StringLength(255)]
        public string MaxGirl { get; set; }

        [StringLength(255)]
        public string MinGirl { get; set; }

        [StringLength(255)]
        public string Before17Min { get; set; }

        [StringLength(255)]
        public string Before17Max { get; set; }

        [StringLength(255)]
        public string After17Min { get; set; }

        [StringLength(255)]
        public string After17Max { get; set; }

        public int? PatientId { get; set; }

        public int? HospitalId { get; set; }

        [StringLength(255)]
        public string Equal { get; set; }

        [StringLength(255)]
        public string Unit { get; set; }

        [StringLength(255)]
        public string DataName { get; set; }

        public int? DataCode { get; set; }

        [StringLength(255)]
        public string Logogram { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }

        public virtual Hospital Hospital { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
