using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TradeWars.Data
{
    public class TradeWarsDbContext : DbContext
    {
        public DbSet<Site> Sites { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Game> Games { get; set; }

        public TradeWarsDbContext(DbContextOptions<TradeWarsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public class TradeWarsContextFactory : IDesignTimeDbContextFactory<TradeWarsDbContext>
        {
            public TradeWarsDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<TradeWarsDbContext>();
                //optionsBuilder.UseSqlite("Data Source=TradeWars.db");
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=TradeWars;Trusted_Connection=True;MultipleActiveResultSets=true");
//                    Configuration.GetConnectionString("TradeWarsConnection")));
//TODO
                return new TradeWarsDbContext(optionsBuilder.Options);
            }
        }
    }
}
