namespace KidneyCareApi.Dal
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Db : DbContext
    {
        public Db()
            : base("name=Db2")
        {
        }

        public virtual DbSet<AuthenCode> AuthenCodes { get; set; }
        public virtual DbSet<DataType> DataTypes { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Nurse> Nurses { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientsData> PatientsDatas { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<table1> table1 { get; set; }
        public virtual DbSet<table2> table2 { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthenCode>()
                .Property(e => e.PhoneNum)
                .IsUnicode(false);

            modelBuilder.Entity<AuthenCode>()
                .Property(e => e.AuthenCode1)
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

            modelBuilder.Entity<Hospital>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Hospital>()
                .Property(e => e.Code)
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

            modelBuilder.Entity<Message>()
                .Property(e => e.Messge)
                .IsUnicode(false);

            modelBuilder.Entity<Nurse>()
                .HasMany(e => e.Patients)
                .WithOptional(e => e.Nurse)
                .HasForeignKey(e => e.BelongToNurse);

            modelBuilder.Entity<PatientsData>()
                .Property(e => e.DataValue)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsData>()
                .Property(e => e.RecordTime)
                .IsUnicode(false);

            modelBuilder.Entity<PatientsData>()
                .Property(e => e.RecordDate)
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

            modelBuilder.Entity<table1>()
                .Property(e => e.test)
                .IsUnicode(false);

            modelBuilder.Entity<table2>()
                .Property(e => e.dd)
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
                .Property(e => e.OpenId)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.FromUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ToUser);
        }
    }
}
