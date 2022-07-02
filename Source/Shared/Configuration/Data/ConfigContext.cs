using Configuration.Models;
using Microsoft.EntityFrameworkCore;

//sqllocaldb info MSSQLLocalDB
//sqllocaldb start

//add-migration InitialMigration -Context MenuContext
//Remove-Migration -Context MenuContext
//Update-Database -Context MenuContext

namespace Configuration.Data;

public class ConfigContext : DbContext
{
    const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;" +
        "Trusted_Connection=True;Database=FirstMate-Config";
          

    public ConfigContext() : base() { }
    //public CondigContext(DbContextOptions<CondigContext> options) : base(options) { }

    //
    public DbSet<Setting> Settings { get; set; }

    //
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }
}
