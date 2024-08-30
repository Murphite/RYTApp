using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RYT.Models.Entities;

namespace RYT.Data
{
    public class RYTDbContext: IdentityDbContext<AppUser>
    {
        public RYTDbContext(DbContextOptions<RYTDbContext> options):base(options)
        {
            
        }

        public DbSet<Wallet>? Wallets { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }
        public DbSet<Notification>? Notifications { get; set; }
        public DbSet<UserChat>? UserChats { get; set; }
        public DbSet<Teacher>? Teachers { get; set; }
        public DbSet<SubjectsTaught>? SubjectsTaughts { get; set; }
        public DbSet<SchoolsTaught>? schoolsTaughts { get; set; }
    }
}
