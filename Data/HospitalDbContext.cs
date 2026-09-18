using Microsoft.EntityFrameworkCore;
using CarePulseERP.Models;

namespace CarePulseERP.Data;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    public DbSet<StaffMember> StaffMembers => Set<StaffMember>();
    public DbSet<SalaryRecord> SalaryRecords => Set<SalaryRecord>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Bed> Beds => Set<Bed>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Staff & Salary Relationship
        modelBuilder.Entity<StaffMember>()
            .HasMany(s => s.SalaryRecords)
            .WithOne(r => r.StaffMember)
            .HasForeignKey(r => r.StaffMemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // Staff & Patient Relationship
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.AssignedDoctor)
            .WithMany(s => s.AssignedPatients)
            .HasForeignKey(p => p.AssignedDoctorId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes for high performance queries
        modelBuilder.Entity<StaffMember>()
            .HasIndex(s => s.Role);

        modelBuilder.Entity<SalaryRecord>()
            .HasIndex(r => new { r.StaffMemberId, r.MonthYear });

        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.Status);
    }
}
