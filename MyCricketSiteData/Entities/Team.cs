using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace MyCricketSiteData.Entities
{
    [BsonIgnoreExtraElements]
    public class Team : MongoEntity
    {
        public Team()
        { }


        [Required]
        public string TeamName { get; set; }

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string HomeGround { get; set; }

    }

    

}
