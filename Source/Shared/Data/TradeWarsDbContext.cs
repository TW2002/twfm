using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TradeWars.Data
{
    public class TWDB : DbContext
    {
        public DbSet<Site> Sites { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Game> Games { get; set; }

        public TWDB(DbContextOptions<TWDB> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TradeWars;Trusted_Connection=True;MultipleActiveResultSets=true");
        //TODO                    Configuration.GetConnectionString("TradeWarsConnection")));
        }


    }
}
