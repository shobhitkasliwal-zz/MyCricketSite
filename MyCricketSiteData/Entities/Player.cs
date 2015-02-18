using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MyCricketSiteData.Entities
{
    [BsonIgnoreExtraElements]
    public class Player : MongoEntity
    {
        public Player()
        { }

        
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public string DateAdded { get; set; }
    }
}
