using Configuration.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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