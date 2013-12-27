using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClientDataModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Linq.Expressions; 
namespace ServerDataModel
{
    
    public class TrackableItem : IConvertToClientData, ICreateFromClientData
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
        
        public T ToClientData<T>() where T : class
        {
            var t = new ClientDataModel.TrackableItem() {Description = Description, Id = Id.ToString(), IsSecured = IsSecured, Name = Name, SupportsGeolocationServices = SupportsGeolocationServices, SupportsUserInformation = SupportsUserInformation, UserId = UserId, States = new List<ClientDataModel.TrackableItemState>()};

            if (States != null)
                t.States = States.Select(x => x.ToClientData<ClientDataModel.TrackableItemState>()).ToList();
            return t as T; 
        }

        public void ApplyClientData(ClientDataBase clientEntity)
        {
            var cti = clientEntity as ClientDataModel.TrackableItem;
            this.Description = cti.Description;
            this.IsSecured = cti.IsSecured;
            this.Name = cti.Name;
            this.SupportsGeolocationServices = cti.SupportsGeolocationServices;
            this.SupportsUserInformation = cti.SupportsUserInformation;
            this.States.ToList().ForEach(x => x.ApplyClientData(cti.States.FirstOrDefault(s => s.Id == x.Id.ToString()))); 
        }
    }

}