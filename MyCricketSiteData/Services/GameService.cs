using MongoDB.Driver.Builders;
using MyCricketSiteData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace MyCricketSiteData.Services
{
    public class GameService : EntityService<Game>
    {

        public Game GetGameByRefId(string refId)
        {

            var entityQuery = Query<Game>.EQ(e => e.RefId, refId);
            return this.DBConnectionHandler.DBCollection.FindOne(entityQuery);


        }

        public List<Game> GetGamesForTournament(string tournamentID)
        {

            //var t = from b in this.DBConnectionHandler.DBCollection.AsQueryable<Game>()
              //      where b.TournamentID == tournamentID select b;

            return this.DBConnectionHandler.DBCollection.AsQueryable<Game>().Where(g => g.TournamentID == tournamentID).ToList<Game>();
            //var tCursor = this.DBConnectionHandler.DBCollection.FindAllAs<Game>().Where(
              //     .SetSortOrder(SortBy<Game>.Descending(g => g.GameDate)).ToList<Game>();

            //var query = Query.And(Query<Game>.EQ(t => t.TournamentID, tournamentID));
                     

            //var tickets = DbCollection.Find(query);
        }

        public override void Update(Game entity)
        {
        }
    }
}
