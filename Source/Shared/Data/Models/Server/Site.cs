using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeWars.Data
{
    public class Site  
    {
        public bool Active { get; set; }
        public int SiteId { get; set; }
        public String Alias { get; set; }
        public string Description { get; set; }
    }
}
