using System;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyCricketSiteData.Entities
{
    public interface IMongoEntity
    {
        ObjectId Id { get; set; }
    }

    public class MongoEntity : IMongoEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
