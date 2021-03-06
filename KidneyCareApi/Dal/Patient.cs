namespace KidneyCareApi.Dal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KidneyCare.Patients")]
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            IndicatorsRanges = new HashSet<IndicatorsRange>();
            PatientsCourses = new HashSet<PatientsCourse>();
            PatientsDatas = new HashSet<PatientsData>();
            PatientsDiseases = new HashSet<PatientsDisease>();
            PatientsDrugs = new HashSet<PatientsDrug>();
            Reports = new HashSet<Report>();
        }

        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? BelongToHospital { get; set; }

        public int? BelongToNurse { get; set; }

        public int? BelongToDoctor { get; set; }

        public int? CKDLeave { get; set; }

        public int? DiseaseType { get; set; }

        [StringLength(255)]
        public string BindStatus { get; set; }

        [StringLength(255)]
        public string LastExceptionDate { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual Hospital Hospital { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IndicatorsRange> IndicatorsRanges { get; set; }

        public virtual Nurse Nurse { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientsCourse> PatientsCourses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientsData> PatientsDatas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientsDisease> PatientsDiseases { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientsDrug> PatientsDrugs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Reports { get; set; }
    }
}
