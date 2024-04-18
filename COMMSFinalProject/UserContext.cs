using Microsoft.EntityFrameworkCore;
using COMMSFinalProject.Models;
using System;

namespace COMMSFinalProject
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=LoanAPIDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");
        }
    }
}
