using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeWars.Data
{
    public interface IDataLayer
    {
        IList<Game> AllGames();
        Game GamesById(int gameid);
    }
    class DataLayer :IDataLayer
    {
        public IList<Game> AllGames()
        {
            using TWDB twdb = new TWDB();
            return twdb.Games.ToList();
        }

        public Game GamesById(int ServerId)
        {
            using TWDB twdb = new TWDB();
            return twdb.Games
                .Single(g => g.ServerId == ServerId);
        }
    }
}
