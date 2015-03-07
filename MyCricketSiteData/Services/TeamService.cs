using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCricketSiteData.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MyCricketSiteData.Services
{
    public class TeamService : EntityService<Team>
    {
        //public IEnumerable<Team> getTeamsForTournament(string tournamentID)
        //{
        //    var tCursor = this.DBConnectionHandler.DBCollection.FindAllAs<Team>()
        //            .SetSortOrder(SortBy<Team>.Descending(g => g.TeamName))
        //            .Where(g => g.TournamentIds.Contains(tournamentID));
        //    return tCursor;
        //}
        public Team GetTeamByName(string name, string tournamentid)
        {

            var entityQuery = Query<Team>.EQ(e => e.TeamName, name);

            return this.DBConnectionHandler.DBCollection.FindOne(entityQuery);


        }


        public override void Update(Team entity)
        {
        }
    }
}
