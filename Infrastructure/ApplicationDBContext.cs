using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Member>       Members { get; set; }
    public DbSet<Department>   Departments { get; set; }
    public DbSet<Subject>      Subjects { get; set; }
    public DbSet<Enrollment>   Enrollments { get; set; }
    public DbSet<User>         Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Member>()
            .HasDiscriminator<string>("MemberType")
            .HasValue<Student>("Student");

        modelBuilder.Entity<Member>()
            .HasOne(m => m.Department)
            .WithMany(d => d.Members)
            .HasForeignKey(m => m.DepartmentId);

        modelBuilder.Entity<Member>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId);

        modelBuilder.Entity<Member>()
            .HasIndex(m => m.UserId)
            .IsUnique();

        modelBuilder.Entity<Member>()
            .HasIndex(m => m.Id)
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