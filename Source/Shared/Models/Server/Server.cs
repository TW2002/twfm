using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeWars.Data
{
    public class Server
    {
        [Key]
        public Guid ServerId { get; set; }
        public Guid SiteId { get; set; }
        public bool Active { get; set; }

        [StringLength(10)]
        public String Alias { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
