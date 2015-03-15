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
    public class TournamentService : EntityService<Tournament>
    {
        public List<Tournament> getAllTournaments()
        {
            var tCursor = this.DBConnectionHandler.DBCollection.FindAllAs<Tournament>()
                    .SetSortOrder(SortBy<Tournament>.Descending(g => g.StartDate)).ToList<Tournament>();


            return tCursor;
        }

        public List<Team> getTournamentTeams(string tournamentID)
        {
            List<ObjectId> TeamIds = new List<ObjectId>();

            Tournament tournament = this.GetById(tournamentID);
            foreach (var groups in tournament.Groups)
            {
                foreach (var grp in groups.Value)
                {
                    TeamIds.Add(new ObjectId(grp.Key.ToString()));
                }
            }
            TeamService tserv = new TeamService();
            return tserv.GetTeambyIds(TeamIds);


        }

        public List<Player> getTournamentPlayers(string tournamentID, string teamId)
        {
            List<ObjectId> playerIds = new List<ObjectId>();

            Tournament tournament = this.GetById(tournamentID);
            foreach (var groups in tournament.Groups)
            {
                foreach (var grp in groups.Value)
                {
                    if (grp.Key.ToString().ToLower().Equals(teamId.ToLower()))
                    {
                        foreach (var pl in grp.Value)
                        {
                            playerIds.Add(new ObjectId(pl.ToString()));
                        }
                    }
                }
            }
            PlayerService pserv = new PlayerService();
            return pserv.GetPlayerbyIds(playerIds);


        }
        public override void Update(Tournament entity)
        {
        }
    }
}
