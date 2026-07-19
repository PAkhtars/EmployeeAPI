using EmployeeAPI.Core.Entities;
using EmployeeAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<ActMaster> ActMasters { get; set; }
        public DbSet<ActDetails> ActDetails { get; set; }
        public DbSet<LegalCategoryMaster> LegalCategoryMasters { get; set; }
        public DbSet<ComplaintMaster> ComplaintMasters { get; set; }
        //public DbSet<Vote> Votes { get; set; }           // Add later
        //public DbSet<PollComment> PollComments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(w => 
                w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);     // 18 total digits, 2 after decimal

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username).IsUnique();

            // === Poll Configuration ===
            modelBuilder.Entity<Poll>(entity =>
            {
                entity.HasKey(p => p.PollId);

                entity.HasOne(p => p.Creator)
                      .WithMany()
                      .HasForeignKey(p => p.CreatorId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // === PollOption Configuration (This was missing!) ===
            modelBuilder.Entity<PollOption>(entity =>
            {
                entity.HasKey(o => o.OptionId);                    // ← Primary Key

                entity.HasOne(o => o.Poll)
                      .WithMany(p => p.Options)
                      .HasForeignKey(o => o.PollId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.Property(o => o.DisplayOrder).IsRequired();
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasKey(v => v.VoteId);
                entity.HasIndex(v => new { v.UserId, v.PollId }).IsUnique(); // Prevent duplicate votes
            });

            // === ActMaster Configuration ===
            modelBuilder.Entity<ActMaster>(entity =>
            {
                entity.HasKey(a => a.ActId);

                // entity.HasOne(a => a.LegalCategoryMaster)
                //       .WithMany()
                //       .HasForeignKey(a => a.LegalCategoryId)
                //       .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}