using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServerDataModel
{
    public class Tracking
    {
        public void Traсking()
        {
            CreatedDate = DateTime.Now;
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [DisplayName("Tracking number")]
        public string TrackingNumber 
        {
            get { return Id.ToString(); }
        }

        public string User { get; set; }
        
        public ObjectId TrackingItemId { get; set; }
        
        public ObjectId StateId { get; set; }
        
        [DisplayName("Comment")]
        public string Comment { get; set;}

        public IList<TrackingHistoryRecord> History { get; set; }

        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }
        
        [DisplayName("Customer Information")]
        public TrackingCustomerInformation CustomerInformation { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }


    }
}