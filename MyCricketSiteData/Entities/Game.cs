using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MyCricketSiteData.Entities
{
    [BsonIgnoreExtraElements]
    public class Game : MongoEntity
    {
        public Game()
        { }

        public string TournamentID { get; set; }
        public DateTime GameDate { get; set; }
        public string GroupName { get; set; }
        public Dictionary<string, string> PlayingTeams { get; set; }
        public Dictionary<string, string> UmpiringTeams { get; set; }
        public string RefId { get; set; }

    }
}
