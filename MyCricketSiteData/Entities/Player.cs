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

        public string BattingStyle { get; set; }

        public string BowlingStyle { get; set; }

        public string FieldingPosition { get; set; }

        //[BsonDateTimeOptions(DateOnly = true)]
        public DateTime DateAdded { get; set; }

        public string ImageUrl { get; set; }

        public string Role { get; set; }
        public string RefId { get; set; }
        public string RefType { get; set; }


    }
}
