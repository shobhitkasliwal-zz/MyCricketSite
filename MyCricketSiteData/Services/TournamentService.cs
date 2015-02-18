using MongoDB.Driver.Builders;
using MyCricketSiteData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCricketSiteData.Services
{
    public class TournamentService:EntityService<Tournament>
    {
        public IEnumerable<Tournament> getActiveTornaments()
        {
            var tCursor = this.DBConnectionHandler.DBCollection.FindAllAs<Tournament>()
                    .SetSortOrder(SortBy<Tournament>.Descending(g => g.StartDate))
                    .Where(g => g.Status == "Active");

            return tCursor;
        }
        public override void Update(Tournament entity)
        {
        }
    }
}
