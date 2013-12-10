using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace ServerDataModel
{
    public class TrackingHistoryRecord
    {
        [Display(Name = "Time Stamp")]
        public DateTime CreatedDate { get; set; }
        
        public ObjectId StateId { get; set; }
        
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        
        [Display(Name = "Latitude")]
        public decimal Latitude { get; set; }
        
        [Display(Name = "Longitude")]
        public decimal Longitude { get; set; }
    
    }
}