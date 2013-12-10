using System;
using System.Runtime.Serialization;

namespace ClientDataModel
{
    [DataContract]
    public class TrackingHistoryRecord
    {
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public string StateId { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
    }
}