using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TournamentGenerator.Models;

namespace TournamentGenerator.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet <ApplicationUser> Users { get; set; }

        public DbSet<Game> Games { get; set; }
        public DbSet<Round> Rounds { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<PhysicalTable> PhysicalTables { get; set; }

        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Game>()
                .HasOne(g => g.PlayerOne)
                .WithMany(p => p.MyGames)
                .HasForeignKey(g => g.PlayerOneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.PlayerTwo)
                .WithMany(p => p.TheirGames)
                .HasForeignKey(g => g.PlayerTwoId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }





}