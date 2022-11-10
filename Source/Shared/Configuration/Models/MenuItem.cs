using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Models;

public class MenuItem
{
    [Key]
    public Guid MenuItemID { get; set; }
    [Required]
    public Guid ParentID { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Key { get; set; }
    [Required]
    public string? Caption { get; set; }
    public string? Command { get; set; }
    public string? Parameters { get; set; }
    public string? Help { get; set; }
}