namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.Report")]
    public partial class Report
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Report()
        {
            PatientsDatas = new HashSet<PatientsData>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string ReportDate { get; set; }

        [StringLength(255)]
        public string ReportMark { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [StringLength(500)]
        public string ImageUrl1 { get; set; }

        [StringLength(500)]
        public string ImageUrl2 { get; set; }

        [StringLength(500)]
        public string ImageUrl3 { get; set; }

        [StringLength(500)]
        public string ImageUrl4 { get; set; }

        [StringLength(500)]
        public string ImageUrl5 { get; set; }

        [StringLength(500)]
        public string ImageUrl6 { get; set; }

        [StringLength(500)]
        public string ImageUrl7 { get; set; }

        [StringLength(500)]
        public string ImageUrl8 { get; set; }

        [Column(TypeName = "blob")]
        public byte[] ImageData { get; set; }

        public int? ReportType { get; set; }

        public int? PatientId { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }

        public virtual Patient Patient { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientsData> PatientsDatas { get; set; }
    }
}
