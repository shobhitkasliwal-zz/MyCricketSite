using MongoDB.Driver.Builders;
using MyCricketSiteData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public override void Update(Tournament entity)
        {
        }
    }
}
