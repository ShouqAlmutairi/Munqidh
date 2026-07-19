using Microsoft.EntityFrameworkCore;
using ScamShieldAI.Models;

namespace ScamShieldAI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AnalyzedMessage> Messages => Set<AnalyzedMessage>();
        public DbSet<KeywordRule> Keywords => Set<KeywordRule>();
        public DbSet<AppUser> Users => Set<AppUser>();
    }
}
