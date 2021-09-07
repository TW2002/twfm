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
        //Game GameById(int gameid);
        IList<Game> GamesByServer(int id);
    }
    class DataLayer :IDataLayer
    {
        public IList<Game> AllGames()
        {
            using TWDB twdb = new TWDB();
            return twdb.Games.ToList();
        }

        public IList<Game> GamesByServer(int id)
        {
            using TWDB twdb = new TWDB();
            return twdb.Games
                .Where(g => g.ServerId == id)
                .ToList();
        }
    }
}
