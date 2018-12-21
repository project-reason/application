using CrossCutting.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace CrossCutting.Logging.Data
{
    public class Log4netDBContext : DbContext
    {
        public virtual DbSet<Log> Logs { get; set; }

        public Log4netDBContext(DbContextOptions<Log4netDBContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}