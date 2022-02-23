using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Enuns;

namespace SchoolOfDevs.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

             protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Note>()
                   .Property(r => r.Value)
                   .HasColumnType("decimal(18,2)"); ;


            builder.Entity<User>()
                .Property(e => e.TypeUser)
                .HasConversion(
                    v => v.ToString(),
                    v => (TypeUser)Enum.Parse(typeof(TypeUser), v));

            builder.Entity<Course>()
               .HasOne(e => e.Teacher)
               .WithMany(c => c.CoursesTeaching)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Course>()
                 .HasMany(p => p.Students)
                 .WithMany(t => t.CoursesStuding)
                 .UsingEntity<StudentCourse>(
                  j => j
                       .HasOne(pt => pt.Student)
                       .WithMany(t => t.StudentCourses)
                       .HasForeignKey(pt => pt.StudentId),
                  j => j
                       .HasOne(pt => pt.Course)
                       .WithMany(p => p.StudentCourses)
                       .HasForeignKey(pt => pt.CourseId),
                  j =>
                  {
                      j.HasKey(t => new { t.CourseId, t.StudentId });
                  });
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                DateTime dateTime = DateTime.Now;
                ((BaseEntity)entityEntry.Entity).UpdatedAt = dateTime;
                if (entityEntry.State == EntityState.Added)
                    ((BaseEntity)entityEntry.Entity).CreatedAt = dateTime;
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public DbSet<User> Users { get; set; }
            public DbSet<Note> Notes { get; set; }
            public DbSet<Course> Courses { get; set; }

    }
}
