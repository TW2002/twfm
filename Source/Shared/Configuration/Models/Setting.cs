using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Models;

public class Setting
{
    [Key]
    public Guid SettingID { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Value { get; set; }
}
