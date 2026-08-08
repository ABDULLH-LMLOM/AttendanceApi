using AttendanceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().Property(s => s.Name).HasMaxLength(20);
            modelBuilder.Entity<Student>().Property(s => s.Department).HasMaxLength(2);
            modelBuilder.Entity<Exam>().Property(e => e.Name).HasMaxLength(20);
            modelBuilder.Entity<Sector>().Property(e => e.Name).HasMaxLength(10);

            modelBuilder.Entity<StudentsExams>().HasKey(se => new { se.StudentId, se.ExamId });

            modelBuilder.Entity<StudentsExams>()
                 .HasOne(se => se.Student)
                 .WithMany(s => s.StudentsExams)
                 .HasForeignKey(se => se.StudentId);

            modelBuilder.Entity<StudentsExams>()
                .HasOne(se => se.Exam)
                .WithMany(e => e.StudentsExams)
                .HasForeignKey(se => se.ExamId);
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<StudentsExams> StudentsExams { get; set; }
        public DbSet<Sector> Sectors { get; set; }

    }
}
