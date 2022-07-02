using Settings.Models;
using Microsoft.EntityFrameworkCore;

namespace Settings.Data;

public class SettingContext : DbContext
{
    public SettingContext(DbContextOptions<SettingContext> options) : base(options)
    {
    }

    public DbSet<Setting> Settings { get; set; }
}
