using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MyCricketSiteData.Entities
{
    [BsonIgnoreExtraElements]
    public class Tournament : MongoEntity
    {
        public Tournament()
        {

        }


        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime StartDate { get; set; }

        [Required]
        public string Name { get; set; }

        public string Status { get; set; }

        Dictionary<string, Dictionary<string, List<string>>> Groups { get; set; }
    }
}
