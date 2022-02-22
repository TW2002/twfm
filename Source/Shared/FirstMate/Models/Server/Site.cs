using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeWars
{
    public class Site  
    {
        [Key]
        public Guid SiteId { get; set; }

        public bool Active { get; set; }

        [StringLength(10)]
        public String Alias { get; set; }

        [StringLength(50)]
        [Column(TypeName = "char(50)")]
        public string Description { get; set; }
    }
}
