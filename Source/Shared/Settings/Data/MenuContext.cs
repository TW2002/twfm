using Settings.Models;
using Microsoft.EntityFrameworkCore;

namespace Settings.Data;

public class MenuContext : DbContext
{
    public MenuContext(DbContextOptions<MenuContext> options) : base(options)
    {
    }

    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
}
