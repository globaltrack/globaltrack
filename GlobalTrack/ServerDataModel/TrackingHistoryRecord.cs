using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace ServerDataModel
{
    public class TrackingHistoryRecord: IConvertToClientData
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

        public T ToClientData<T>() where T : class
        {
            var t = new ClientDataModel.TrackingHistoryRecord
            {
                Comment = Comment,
                CreatedDate = CreatedDate,
                Latitude = Latitude,
                Longitude = Longitude,
                StateId = StateId.ToString()
            };
            return t as T; 
        }
    }
}