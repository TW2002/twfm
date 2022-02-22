using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeWars
{
    public class Game
    {
        [Key]
        public Guid GameId { get; set; }
        public Guid ServerId { get; set; }
        public bool Active { get; set; }

        [StringLength(1)]
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Game Slot")]
        public String Slot { get; set; }

        [StringLength(120)]
        [Display(Name = "Game Name")]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "Big Bang")]
        public DateTime Started { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
