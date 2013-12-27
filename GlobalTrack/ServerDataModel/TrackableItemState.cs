using System.ComponentModel.DataAnnotations;
using ClientDataModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServerDataModel
{
    public class TrackableItemState : IConvertToClientData, ICreateFromClientData 
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description {get; set;}


        public T ToClientData<T>() where T:class  
        {
            return new ClientDataModel.TrackableItemState() { Id = Id.ToString(), Description = Description, Name = Name } as T; 
        }

        public void ApplyClientData(ClientDataBase clientEntity)
        {
            var cti = clientEntity as ClientDataModel.TrackableItemState;

            this.Name = cti.Name;
            this.Description = cti.Description; 
        }
    }
}