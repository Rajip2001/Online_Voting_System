using Microsoft.EntityFrameworkCore;
using OnlineVoting.Models;
using System;

namespace OnlineVoting.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     // ================================
        //     // Seed Users
        //     // ================================
        //     modelBuilder.Entity<User>().HasData(
        //         new User
        //         {
        //             UserId = 1,
        //             Name = "Admin User",
        //             Email = "admin@example.com",
        //             Password = "Password123",
        //             CITNo = "CIT123456",
        //             DOB = new DateTime(1990, 1, 1),
        //             Role = "Admin",
        //             IsCandidate = false
        //         },
        //         new User
        //         {
        //             UserId = 2,
        //             Name = "John Doe",
        //             Email = "john@example.com",
        //             Password = "Password123",
        //             CITNo = "CIT654321",
        //             DOB = new DateTime(2000, 5, 15),
        //             Role = "Voter",
        //             IsCandidate = false
        //         },
        //         new User
        //         {
        //             UserId = 3,
        //             Name = "Jane Smith",
        //             Email = "jane@example.com",
        //             Password = "Password123",
        //             CITNo = "CIT987654",
        //             DOB = new DateTime(1999, 8, 20),
        //             Role = "Voter",
        //             IsCandidate = false
        //         }
        //     );

        //     // ================================
        //     // Seed Administrator
        //     // ================================
        //     modelBuilder.Entity<Administrator>().HasData(
        //         new Administrator { AdminId = 1, UserId = 1 });

        //     // ================================
        //     // Seed Elections
        //     // ================================
        //     modelBuilder.Entity<Election>().HasData(
        //     new Election
        //     {
        //         ElectionId = 1,
        //         Name = "Student Council Election 2026",
        //         Status = "Ongoing", // Required field
        //         StartDate = new DateTime(2026, 3, 10, 10, 0, 0),
        //         EndDate = new DateTime(2026, 3, 17, 10, 0, 0)
        //     }
        //         );

        //     // ================================
        //     // Seed Candidates
        //     // ================================
        //     modelBuilder.Entity<Candidate>().HasData(
        //     new Candidate
        //     {
        //         CandidateId = 1,
        //         Name = "Alice Candidate",
        //         Age = 25,
        //         Education = "BSc Computer Science",
        //         CITNo = "CIT1001",
        //         Party = "Party A",
        //         Logo = "logo1.png",
        //         ProfilePicture = "profile1.png",
        //         VoteCount = 0,
        //         Manifesto = "I promise transparency",
        //         ElectionId = 1
        //     },
        //     new Candidate
        //     {
        //         CandidateId = 2,
        //         Name = "Bob Candidate",
        //         Age = 27,
        //         Education = "BBA",
        //         CITNo = "CIT1002",
        //         Party = "Party B",
        //         Logo = "logo2.png",
        //         ProfilePicture = "profile2.png",
        //         VoteCount = 0,
        //         Manifesto = "I will improve student facilities",
        //         ElectionId = 1
        //     }
        // );

        //     // ================================
        //     // Seed Votes (optional)
        //     // ================================
        //     // Uncomment if you want to pre-populate votes
        //     /*
        //     modelBuilder.Entity<Vote>().HasData(
        //         new Vote { VoteId = 1, UserId = 2, CandidateId = 1, ElectionId = 1, VotedAt = DateTime.UtcNow },
        //         new Vote { VoteId = 2, UserId = 3, CandidateId = 2, ElectionId = 1, VotedAt = DateTime.UtcNow }
        //     );
        //     */
        // }
    }
}