using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GlobalTrackService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IGlobalTrackServicev1
    {

        [OperationContract]
        LoginResponse Login(string userName, string password);

        [OperationContract]
        IList<ClientDataModel.Tracking> GetTrackings(string sessionId, DateTime? from, DateTime? to, string searchString);

        [OperationContract]
        IList<ClientDataModel.TrackableItem> GetTrackableItems(string sessionId);

        [OperationContract]
        ClientDataModel.TrackableItem GetTrackableItem(string sessionId, string objectId);

        [OperationContract]
        ClientDataModel.TrackableItem UpdateTrackableItem(string sessionId, ClientDataModel.TrackableItem trackableItem);
        

    }

    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public string SessionId { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

}
