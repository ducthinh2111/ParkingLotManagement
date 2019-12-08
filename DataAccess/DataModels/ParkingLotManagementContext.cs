using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.DataModels
{
    public partial class ParkingLotManagementContext : DbContext
    {
        public ParkingLotManagementContext()
        {
        }

        public ParkingLotManagementContext(DbContextOptions<ParkingLotManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminAccount> AdminAccount { get; set; }
        public virtual DbSet<Block> Block { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<MonthlyIncome> MonthlyIncome { get; set; }
        public virtual DbSet<Parking> Parking { get; set; }
        public virtual DbSet<ParkingLot> ParkingLot { get; set; }
        public virtual DbSet<Slot> Slot { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ParkingLotManagementDatabase"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<AdminAccount>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Block>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ParkingLotID)
                    .HasColumnName("ParkingLotID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParkingLot)
                    .WithMany(p => p.Block)
                    .HasForeignKey(d => d.ParkingLotID)
                    .HasConstraintName("FK__Block__ParkingLo__398D8EEE");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BirthDay).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.isVIP)
                    .HasColumnName("isVIP")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ParkingLotID)
                    .HasColumnName("ParkingLotID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParkingLot)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.ParkingLotID)
                    .HasConstraintName("FK__Customer__Parkin__4222D4EF");
            });

            modelBuilder.Entity<MonthlyIncome>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Month)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Parking>(entity =>
            {
                entity.HasKey(e => e.LicensePlate)
                    .HasName("PK__Parking__026BC15D20D69EF4");

                entity.Property(e => e.LicensePlate)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CheckInDateTime).HasColumnType("datetime");

                entity.Property(e => e.CheckOutDateTime).HasColumnType("datetime");

                entity.Property(e => e.SecurityCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SlotID)
                    .HasColumnName("SlotID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.Parking)
                    .HasForeignKey(d => d.SlotID)
                    .HasConstraintName("FK__Parking__SlotID__4BAC3F29");
            });

            modelBuilder.Entity<ParkingLot>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.VIPDiscount).HasColumnName("VIPDiscount");

                entity.Property(e => e.VIPRequiredTime).HasColumnName("VIPRequiredTime");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Availability)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BlockID)
                    .HasColumnName("BlockID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.Slot)
                    .HasForeignKey(d => d.BlockID)
                    .HasConstraintName("FK__Slot__BlockID__3C69FB99");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BirthDay).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ParkingLotID)
                    .HasColumnName("ParkingLotID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParkingLot)
                    .WithMany(p => p.Staff)
                    .HasForeignKey(d => d.ParkingLotID)
                    .HasConstraintName("FK__Staff__ParkingLo__44FF419A");
            });
        }
    }
}
