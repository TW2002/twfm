﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Models;

public class Menu
{
    [Key]
    public Guid MenuID { get; set; }
    public Guid? ParentID { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Key { get; set; }
    [Required]
    public string? Caption { get; set; }
    [Required]
    public string? Prompt { get; set; }
    public string? Help { get; set; }

    public virtual ObservableCollection<MenuItem>? MenuItems { get; private set; }
}
