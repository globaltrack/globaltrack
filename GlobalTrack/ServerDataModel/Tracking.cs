using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServerDataModel
{
    [DataServiceKey("Id")]
    [DataServiceEntity]
    public class Tracking: IConvertToClientData
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



        public T ToClientData<T>() where T : class
        {
            var t = new ClientDataModel.Tracking();

            t.Comment = this.Comment;
            t.CreatedDate = this.CreatedDate;
            //t.History = this.History; 
            t.Id = this.Id.ToString();
            t.StateId = this.StateId.ToString();
            t.StateName = "NO STATE NAME";
            t.TrackingItemId = this.TrackingItemId.ToString();
            t.TrackingItemName = "NO TRACKING ITEM NAME";
            t.User = this.User; 
            t.History = History.Select( h => h.ToClientData<ClientDataModel.TrackingHistoryRecord>()).ToList();
            return t as T; 
        }
    }
}