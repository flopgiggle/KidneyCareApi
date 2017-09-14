namespace KidneyCareApi.Dal
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Db : DbContext
    {
        public Db()
            : base("name=Db11")
        {
        }

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AuthenCode> AuthenCodes { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<DataType> DataTypes { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<IndicatorsRange> IndicatorsRanges { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Nurse> Nurses { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientsCourse> PatientsCourses { get; set; }
        public virtual DbSet<PatientsData> PatientsDatas { get; set; }
        public virtual DbSet<PatientsDisease> PatientsDiseases { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ZipCode> ZipCodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>()
                .Property(e => e.AreaCode)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.AreaName)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.CityCode)
                .IsUnicode(false);

            modelBuilder.Entity<AuthenCode>()
                .Property(e => e.PhoneNum)
                .IsUnicode(false);

            modelBuilder.Entity<AuthenCode>()
                .Property(e => e.AuthenCode1)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.CityCode)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.CityName)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.ProvinceCode)
                .IsUnicode(false);

            modelBuilder.Entity<DataType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<DataType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .HasMany(e => e.Patients)
                .WithOptional(e => e.Doctor)
                .HasForeignKey(e => e.BelongToDoctor);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<Hospital>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Hospital>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Hospital>()
                .Property(e => e.CityCode)
                .IsUnicode(false);

            modelBuilder.Entity<Hospital>()
                .HasMany(e => e.Doctors)
                .WithOptional(e => e.Hospital)
                .HasForeignKey(e => e.BelongToHospital);

            modelBuilder.Entity<Hospital>()
                .HasMany(e => e.Nurses)
                .WithOptional(e => e.Hospital)
                .HasForeignKey(e => e.BelongToHospital);

            modelBuilder.Entity<Hospital>()
                .HasMany(e => e.Patients)
                .WithOptional(e => e.Hospital)
                .HasForeignKey(e => e.BelongToHospital);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.Min)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.Max)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.MaxGirl)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.MinGirl)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.Before17Min)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.Before17Max)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.After17Min)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.After17Max)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.Equal)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.Unit)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.DataName)
                .IsUnicode(false);

            modelBuilder.Entity<IndicatorsRange>()
                .Property(e => e.Logogram)
                .IsUnicode(false);

            modelBuilder.Entity<Message>()
                .Property(e => e.Messge)
                .IsUnicode(false);

            modelBuilder.Entity<Nurse>()
                .HasMany(e => e.Patients)
                .WithOptional(e => e.Nurse)
                .HasForeignKey(e => e.BelongToNurse);

            modelBuilder.Entity<Patient>()
                .Property(e => e.BindStatus)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.PatientsCourses)
                .WithOptional(e => e.Patient)
                .HasForeignKey(e => e.PaitentId);

            modelBuilder.Entity<PatientsCourse>()
                .Property(e => e.CoursCode)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsCourse>()
                .Property(e => e.CoursName)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsCourse>()
                .Property(e => e.AttendingDates)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsCourse>()
                .Property(e => e.ObjectName)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsCourse>()
                .Property(e => e.ModeName)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsCourse>()
                .Property(e => e.CognitionName)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsCourse>()
                .Property(e => e.BehaviorName)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsCourse>()
                .Property(e => e.Mark)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsData>()
                .Property(e => e.DataValue)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsData>()
                .Property(e => e.RecordTime)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsData>()
                .Property(e => e.RecordDate)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsData>()
                .Property(e => e.Unit)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsDisease>()
                .Property(e => e.DiseaseType)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsDisease>()
                .Property(e => e.DiseaseName)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsDisease>()
                .Property(e => e.DiseaseStartTime)
                .IsUnicode(false);

            modelBuilder.Entity<Province>()
                .Property(e => e.ProvinceCode)
                .IsUnicode(false);

            modelBuilder.Entity<Province>()
                .Property(e => e.ProvinceName)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ReportDate)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ReportMark)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ImageUrl1)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ImageUrl2)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ImageUrl3)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ImageUrl4)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ImageUrl5)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ImageUrl6)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ImageUrl7)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.ImageUrl8)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.NickName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.MobilePhone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Birthday)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.IdCard)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.OpenId)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Profile)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.FromUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ToUser);

            modelBuilder.Entity<ZipCode>()
                .Property(e => e.AreaCode)
                .IsUnicode(false);

            modelBuilder.Entity<ZipCode>()
                .Property(e => e.ZipName)
                .IsUnicode(false);

            modelBuilder.Entity<ZipCode>()
                .Property(e => e.Code)
                .IsUnicode(false);
        }
    }
}
