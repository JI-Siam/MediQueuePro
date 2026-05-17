using System;
using System.Collections.Generic;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public partial class MediQueueProDbContext : DbContext
{
    public MediQueueProDbContext()
    {
    }

    public MediQueueProDbContext(DbContextOptions<MediQueueProDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=MediQueueProDB;User Id=sa;Password=StrongPass123!;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE488CB7EA8D4");

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534BE4922D0").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC23E2394E9");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsEmergency).HasDefaultValue(false);
            entity.Property(e => e.IsVisited).HasDefaultValue(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.VisitTime).HasColumnType("datetime");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Doctors");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Patients");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EBF80E33C3E");

            entity.HasIndex(e => e.Email, "UQ__Doctors__A9D10534358D9827").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Honorarium).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.Specialization).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctors_Specializations");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC366DBFA9987");

            entity.HasIndex(e => e.Email, "UQ__Patients__A9D10534E9364AC7").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BloodGroup).HasMaxLength(10);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.SpecializationId).HasName("PK__Speciali__5809D86F93293302");

            entity.HasIndex(e => e.Name, "UQ__Speciali__737584F6A45DE086").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
