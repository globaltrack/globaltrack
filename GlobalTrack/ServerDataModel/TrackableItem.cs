using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServerDataModel
{
    
    public class TrackableItem
    {
        public string UserId { get; set; }
        [BsonId]
        public ObjectId Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set;}
        [Required]
        [Display(Name = "Password")]
        public bool IsSecured { get; set; }

        [Display(Name = "Geolocation support")]
        public bool SupportsGeolocationServices { get; set; }

        [Display(Name = "User information support")]
        public bool SupportsUserInformation { get; set; }

        public IList<TrackableItemState> States { get; set; }
    }

}