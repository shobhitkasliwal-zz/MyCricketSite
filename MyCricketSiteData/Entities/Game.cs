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

        [BsonDateTimeOptions(DateOnly = true)]
        [Required]
        public DateTime GameDate { get; set; }

        [Required]
        public string TournamentID { get; set; }

        public string[] PlayingTeamIds { get; set; }

        public string[] UmpiringTeamIds { get; set; }

        public string GroundAddress { get; set; }
    }
}
