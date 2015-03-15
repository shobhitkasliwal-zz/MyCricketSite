using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCricketSiteData.Entities;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace MyCricketSiteData.Services
{
    public class PlayerService : EntityService<Player>
    {
        public List<Player> GetPlayerbyIds(List<ObjectId> playerIds)
        {


            return this.DBConnectionHandler.DBCollection.AsQueryable().OfType<Player>().Where(a => playerIds.Contains(a.Id)).ToList<Player>();



        }

        public override void Update(Player entity)
        {
        }
    }
}
