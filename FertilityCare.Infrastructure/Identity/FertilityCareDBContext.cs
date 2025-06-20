using FertilityCare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Identity
{
    public class FertilityCareDBContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        private static readonly string DB_CONNECTION_STRING = "Server=localhost,1433;Database=fertility_care_db;UID=sa;PWD=12345;TrustServerCertificate=True;Encrypt=false";

        //public FertilityCareDBContext(DbContextOptions<FertilityCareDBContext> options)
        //   : base(options)
        //{
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DB_CONNECTION_STRING);

            optionsBuilder.UseLazyLoadingProxies();

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }

        public DbSet<OrderStepPayment> orderStepPayments { get; set; }

        public DbSet<TreatmentService> TreatmentServices { get; set; }

        public DbSet<TreatmentStep> TreatmentSteps { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<MedicalExamination> MedicalExaminations { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderStep> OrderSteps { get; set; }

        public DbSet<Prescription> Prescriptions { get; set; }

        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }

        public DbSet<EmbryoGained> EmbryoGaineds { get; set; }

        public DbSet<EggGained> EggGaineds { get; set; }

        public DbSet<EmbryoTransfer> EmbryoTransfers { get; set; }

        public DbSet<Slot> Slots { get; set; }
    }
}
