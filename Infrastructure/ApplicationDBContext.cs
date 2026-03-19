using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Student>      Students { get; set; }
    public DbSet<Department>   Departments { get; set; }
    public DbSet<Subject>      Subjects { get; set; }
    public DbSet<Enrollment>   Enrollments { get; set; }
    public DbSet<User>         Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Department)
            .WithMany(d => d.Students)
            .HasForeignKey(s => s.DepartmentId);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId);
        
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.UserId)
            .IsUnique();

        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Id)
            .IsUnique();

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Subject)
            .WithMany()
            .HasForeignKey(e => e.SubjectCode);
    }

}