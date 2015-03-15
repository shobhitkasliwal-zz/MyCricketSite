using MongoDB.Driver.Builders;
using MyCricketSiteData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCricketSiteData.Services
{
    public class UserService : EntityService<User>
    {
        public User FindByProvider(string key, string name)
        {

            var entityQuery = Query.And(
                Query<User>.EQ(e => e.ProviderKey, key),
                Query<User>.EQ(e => e.Issuer, name)
                );
            return this.DBConnectionHandler.DBCollection.FindOne(entityQuery);


        }

        public override void Update(User entity)
        {
            this.DBConnectionHandler.DBCollection.Save(entity);

        }
    }
}
