using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Labb3_Anropa_databasen.Models
{
    public partial class HappyValleyContext : DbContext
    {
        public HappyValleyContext()
        {
        }

        public HappyValleyContext(DbContextOptions<HappyValleyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<English> English { get; set; } = null!;
        public virtual DbSet<Math> Math { get; set; } = null!;
        public virtual DbSet<Programming> Programming { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Staff> Staff { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB; Database=HappyValleyTest; Trusted_Connection=True; MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Teacher1)
                    .WithMany()
                    .HasForeignKey(d => d.Teacher1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Courses__Teacher__276EDEB3");

                entity.HasOne(d => d.Teacher2)
                    .WithMany()
                    .HasForeignKey(d => d.Teacher2Id)
                    .HasConstraintName("FK__Courses__Teacher__286302EC");

                entity.HasOne(d => d.Teacher3)
                    .WithMany()
                    .HasForeignKey(d => d.Teacher3Id)
                    .HasConstraintName("FK__Courses__Teacher__29572725");
            });

            modelBuilder.Entity<English>(entity =>
            {
                entity.Property(e => e.Grade).IsFixedLength();
            });

            modelBuilder.Entity<Math>(entity =>
            {
                entity.Property(e => e.Grade).IsFixedLength();
            });

            modelBuilder.Entity<Programming>(entity =>
            {
                entity.Property(e => e.Grade).IsFixedLength();
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Ssn).IsFixedLength();
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.Ssn).IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
