using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCricketSiteData.Entities;
using MongoDB.Driver;
using MyCricketSiteData.Properties;

namespace MyCricketSiteData
{

    public class DbContext<T> where T : IMongoEntity
    {
        public MongoCollection<T> DBCollection { get; private set; }

        public DbContext()
        {
         

            //// Get a thread-safe client object by using a connection string
            var mongoClient = new MongoClient(Settings.Default.MyConnectionString);

            //// Get a reference to a server object from the Mongo client object
            var mongoServer = mongoClient.GetServer();

           
            var db = mongoServer.GetDatabase(Settings.Default.MyDatabaseName);

            //// Get a reference to the collection object from the Mongo database object
            //// The collection name is the type converted to lowercase + "s"
            DBCollection = db.GetCollection<T>(typeof(T).Name.ToLower() + "s");
        }
    }
}
