using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GlobalTrack.Models.Security
{
    public class User
    {
        [BsonId]
        public ObjectId UserId { get; set; }
        public string Name { get; set; }
        public string NameLowerSpace { get; set; }
        public string Password { get; set;}

    }
}