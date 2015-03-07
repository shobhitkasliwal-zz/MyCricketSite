using MongoDB.Driver.Builders;
using MyCricketSiteData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCricketSiteData.Services
{
    public class GameService : EntityService<Game>
    {

        public Game GetGameByRefId(string refId)
        {

            var entityQuery = Query<Game>.EQ(e => e.RefId, refId);
            return this.DBConnectionHandler.DBCollection.FindOne(entityQuery);


        }

        public override void Update(Game entity)
        {
        }
    }
}
