using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeWars.Data
{
    public class Game
    {
        public bool Active { get; set; }
        public int GameId { get; set; }
        public int ServerId { get; set; }
        public String Letter { get; set; }
        public string Description { get; set; }
    }
}
