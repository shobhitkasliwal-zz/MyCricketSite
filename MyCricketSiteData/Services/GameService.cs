using MongoDB.Driver.Builders;
using MyCricketSiteData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

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


        public Dictionary<string, Dictionary<string, object>> GetGameDetailByDate(string tournamentid, DateTime dt)
        {
            Dictionary<string, Dictionary<string, object>> returnObj = new Dictionary<string, Dictionary<string, object>>();
            List<Game> games = this.DBConnectionHandler.DBCollection.AsQueryable<Game>().Where(g => g.TournamentID == tournamentid && g.GameDate == dt).ToList<Game>();
            foreach (Game gm in games)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>();
                obj.Add("Game", obj);
                var HomeTeamID = gm.PlayingTeams["Home"];
                var AwayTeamID = gm.PlayingTeams["Away"];
                var Umpire1TeamID = gm.UmpiringTeams["Umpire1"];
                var Umpire2TeamID = gm.UmpiringTeams["Umpire2"];
                List<Team> lsttm = this.DBConnectionHandler.DBCollection.AsQueryable<Team>().Where(g => g.Id == new ObjectId(HomeTeamID) || g.Id == new ObjectId(AwayTeamID) || g.Id == new ObjectId(Umpire1TeamID) || g.Id == new ObjectId(Umpire2TeamID)).ToList<Team>();
                foreach (Team tm in lsttm)
                {

                    if (tm.EntityId.Equals(HomeTeamID))
                    {
                        obj.Add("HomeTeam", tm);
                    }
                    else if (tm.EntityId.Equals(AwayTeamID))
                    {
                        obj.Add("AwayTeam", tm);
                    }
                    else if (tm.EntityId.Equals(Umpire1TeamID))
                    {
                        obj.Add("Umpire1Team", tm);
                    }
                    else if (tm.EntityId.Equals(Umpire2TeamID))
                    {
                        obj.Add("Umpire2Team", tm);
                    }

                }
                returnObj.Add("Teams", obj);
            }

            return returnObj;

        }

        public override void Update(Game entity)
        {
        }
    }
}
