using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using ServerDataModel;

namespace GlobalTrack.Models.ServerDataModel
{
    public class TrackingViewModel : Tracking
    {
        [BsonIgnore]
        public IEnumerable<SelectListItem> TrackingItems { get; set; }
        [BsonIgnore]
        public IEnumerable<SelectListItem> TrackingItemStates { get; set; }
        [BsonIgnore]
        public string TrackingItemName { get; set; }


    }
}