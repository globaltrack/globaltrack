using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Runtime.Serialization;

namespace ClientDataModel
{
    [DataContract]
    [DataServiceKey("Id")]
    [DataServiceEntity]
    public class Tracking
    {
        
        public void Traсking()
        {
            CreatedDate = DateTime.Now;
        }

        [DataMember]
        public string Id { get; set; }
        public string TrackingNumber
        {
            get { return Id; }
        }
        [DataMember]
        public string User { get; set; }
        [DataMember]
        public string TrackingItemId { get; set; }
        
        [DataMember]
        public string TrackingItemName { get; set; }

        [DataMember]
        public string StateId { get; set; }

        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public IList<TrackingHistoryRecord> History { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }

    }
}
