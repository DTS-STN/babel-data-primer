using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataPrimer.Storage
{
    public partial class BabeldbContext : DbContext
    {
        private readonly string _conn;
        public BabeldbContext(string conn)
        {
            _conn = conn;
        }

        public BabeldbContext(DbContextOptions<BabeldbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CliRoe> CliRoe { get; set; }
        public virtual DbSet<Earnings> Earnings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_conn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CliRoe>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cli_roe");

                entity.Property(e => e.ApplicationStartedDate).HasColumnName("APPLICATION_STARTED_DATE");

                entity.Property(e => e.ClientId).HasColumnName("CLIENT_ID");

                entity.Property(e => e.EducationLevel)
                    .IsRequired()
                    .HasColumnName("EDUCATION_LEVEL")
                    .HasMaxLength(50);

                entity.Property(e => e.EducationLevelId)
                    .IsRequired()
                    .HasColumnName("EDUCATION_LEVEL_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.FinalPayPeriodEndDate).HasColumnName("FINAL_PAY_PERIOD_END_DATE");

                entity.Property(e => e.FirstDayWorkedDate).HasColumnName("FIRST_DAY_WORKED_DATE");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("GENDER")
                    .HasMaxLength(50);

                entity.Property(e => e.GenderId).HasColumnName("GENDER_ID");

                entity.Property(e => e.LanguageId)
                    .IsRequired()
                    .HasColumnName("LANGUAGE_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.LanguageSpoken)
                    .IsRequired()
                    .HasColumnName("LANGUAGE_SPOKEN")
                    .HasMaxLength(50);

                entity.Property(e => e.LastDayPaidDate).HasColumnName("LAST_DAY_PAID_DATE");

                entity.Property(e => e.Municipality)
                    .IsRequired()
                    .HasColumnName("MUNICIPALITY")
                    .HasMaxLength(50);

                entity.Property(e => e.PayPeriodType)
                    .IsRequired()
                    .HasColumnName("PAY_PERIOD_TYPE")
                    .HasMaxLength(50);

                entity.Property(e => e.PayPeriodTypeId).HasColumnName("PAY_PERIOD_TYPE_ID");

                entity.Property(e => e.PersonUid)
                    .IsRequired()
                    .HasColumnName("PERSON_UID")
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasColumnName("POSTAL_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasColumnName("PROVINCE")
                    .HasMaxLength(50);

                entity.Property(e => e.RoeId).HasColumnName("ROE_ID");

                entity.Property(e => e.TotalInsurableEarningAmt).HasColumnName("TOTAL_INSURABLE_EARNING_AMT");

                entity.Property(e => e.TotalInsurableHourNbr).HasColumnName("TOTAL_INSURABLE_HOUR_NBR");

                entity.Property(e => e.YearOfBirth).HasColumnName("YEAR_OF_BIRTH");
            });

            modelBuilder.Entity<Earnings>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("earnings");

                entity.Property(e => e.InsurableEarningAmt)
                    .IsRequired()
                    .HasColumnName("INSURABLE_EARNING_AMT")
                    .HasMaxLength(50);

                entity.Property(e => e.PayPeriodNbr)
                    .IsRequired()
                    .HasColumnName("PAY_PERIOD_NBR")
                    .HasMaxLength(50);

                entity.Property(e => e.RoeId).HasColumnName("ROE_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
