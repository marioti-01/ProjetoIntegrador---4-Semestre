using EduSecure.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EduSecure.Api.Data;

public class EduSecureContext(DbContextOptions<EduSecureContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Professor> Professors => Set<Professor>();
    public DbSet<Discipline> Disciplines => Set<Discipline>();
    public DbSet<ClassRoom> Classes => Set<ClassRoom>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<SecurityLog> SecurityLogs => Set<SecurityLog>();
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Student>().HasIndex(x => x.RegistrationNumber).IsUnique();
        modelBuilder.Entity<Professor>().HasIndex(x => x.EmployeeNumber).IsUnique();
        modelBuilder.Entity<Discipline>().HasIndex(x => x.Code).IsUnique();
        modelBuilder.Entity<Enrollment>().HasIndex(x => new { x.StudentId, x.ClassRoomId }).IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(x => x.Student).WithOne(x => x.User)
            .HasForeignKey<Student>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(x => x.Professor).WithOne(x => x.User)
            .HasForeignKey<Professor>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(x => x.Grade).WithOne(x => x.Enrollment)
            .HasForeignKey<Grade>(x => x.EnrollmentId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Grade>().Property(x => x.Value).HasPrecision(4, 2);
    }
}
