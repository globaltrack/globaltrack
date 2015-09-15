using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using ClientDataModel;
using GlobalTrackService.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GlobalTrackService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GlobalTrackServicev1 : IGlobalTrackServicev1
    {
        private string connectionString ;
        private MongoDatabase _database;
        private MongoCollection<ServerDataModel.Tracking> _trackingCollection;
        private MongoCollection<ServerDataModel.TrackableItem> _trackableItemsCollection;
        private IList<SessionContext> Sessions  = new List<SessionContext>(); 
        public GlobalTrackServicev1()
        {
            connectionString = ConfigurationManager.AppSettings["mongoDbServerConnection"];
            var dbName = ConfigurationManager.AppSettings["mongoDbName"]; 
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(dbName);
            _database = database;
            _trackingCollection = database.GetCollection<ServerDataModel.Tracking>("Trackings");
            _trackableItemsCollection = database.GetCollection<ServerDataModel.TrackableItem>("TrackableItems"); 
        }

        private bool  ValidateUser(string SessionId)
        {

            return Sessions.Any(x => x.SessionId == SessionId); 
        }

        private string GetUser(string session)
        {
            var us = Sessions.FirstOrDefault(x => x.SessionId == session);
            us.LastActivity = DateTime.Now; 
            return us.UserId; 
        }

    
       public IList<ClientDataModel.Tracking> GetTrackings(string sessionId, DateTime? @from, DateTime? to, string searchString)
       {
           if (!ValidateUser(sessionId))
               throw new Exception("Not authorized");
           string userId = GetUser(sessionId); 

           var tiList = _trackableItemsCollection.AsQueryable().Where(x => x.UserId == userId).ToList();
            
           return _trackingCollection.AsQueryable().Where(x => x.User == userId).Select(t => new ClientDataModel.Tracking()
             {Comment = t.Comment, 
                 CreatedDate = t.CreatedDate, 
                 Id = t.Id.ToString(), 
                 User = t.User, 
                 TrackingItemName = tiList.Where(ti => ti.Id == t.TrackingItemId).Select( x=> x.Name).FirstOrDefault(),
              StateName = tiList.FirstOrDefault(ti => ti.Id == t.TrackingItemId).States.FirstOrDefault(x=> x.Id == t.StateId).Name, 
              History = t.History.Select( h => new ClientDataModel.TrackingHistoryRecord()
                  {
                      Comment = h.Comment, 
                      CreatedDate = h.CreatedDate, 
                      Longitude = h.Longitude, 
                      Latitude = h.Latitude,
                      StateId = tiList.FirstOrDefault(ti => ti.Id == t.TrackingItemId).States.FirstOrDefault(s => s.Id == h.StateId).Name
                  }).ToList()
             }).ToList(); 
        }

       public IList<ClientDataModel.TrackableItem> GetTrackableItems(string sessionId)
        {
           if (!ValidateUser(sessionId))
               throw new Exception("Not authorized");
           string userId = GetUser(sessionId);
           var tiList = _trackableItemsCollection.AsQueryable().Where(x => x.UserId == userId).ToList();

            return tiList.Select(ti => ti.ToClientData<ClientDataModel.TrackableItem>()).ToList(); 

        }

        public TrackableItem GetTrackableItem(string sessionId, string objectId)
        {
            if (!ValidateUser(sessionId))
                throw new Exception("Not authorized");
            string userId = GetUser(sessionId);
            ObjectId id;
            if (!ObjectId.TryParse(objectId, out id))
                throw new Exception("Id format is invalid"); 


            var ti  = _trackableItemsCollection.AsQueryable().Where(x => x.UserId == userId).FirstOrDefault(x => x.Id == id);
            var cti = ti == null? null : ti.ToClientData<ClientDataModel.TrackableItem>();
            return cti; 
        }

        public TrackableItem UpdateTrackableItem(string sessionId, TrackableItem clientTrackableItem)
        {
            if (!ValidateUser(sessionId))
                throw new Exception("Not authorized");
            ObjectId  clietTiId; 
             if (!ObjectId.TryParse(clientTrackableItem.Id, out  clietTiId))
                 throw  new Exception("Invalid Trackable Item ID");

            string userId = GetUser(sessionId);
            var serverTi =
                _trackableItemsCollection.AsQueryable()
                                         .FirstOrDefault(
                                             x =>
                                             x.Id == clietTiId && x.UserId == userId);  
            
            if (serverTi == null)
            {
                throw new Exception("TrackableItem with specified id not found in the database");
            }
            
            if (serverTi.States == null ) serverTi.States = new List<ServerDataModel.TrackableItemState>();
            clientTrackableItem.States.ToList().ForEach(cst => 
            {
                //if server entity doesnt contain any client entity - add entity for server. 
                //however if it is and client's one has an ID set - generate error. 
                if (serverTi.States.FirstOrDefault(ss => ss.Id.ToString() == cst.Id) == null)
                {
                    if (string.IsNullOrEmpty(cst.Id))
                    {
                        ServerDataModel.TrackableItemState sState = new ServerDataModel.TrackableItemState() { Id = ObjectId.GenerateNewId() };
                        serverTi.States.Add(sState);
                        cst.Id = sState.Id.ToString();
                    }
                    else
                    {
                        throw new Exception(string.Format("The state with id {0} wasn't found for trackable item {1}. In case adding new state leave the Id field empty." , cst.Id, clientTrackableItem.Id));
                    }

                }

            });

            //Applying client data
            serverTi.ApplyClientData(clientTrackableItem);
            _trackableItemsCollection.Save(serverTi);
            
            //return a result to user 
            return serverTi.ToClientData<TrackableItem>(); 

        }

        public LoginResponse Login(string userName, string password)
        {
            //chech user premissoins. 
            MongoMembershipProvider mp = new MongoMembershipProvider();

            if (mp.ValidateUser(userName, password) )
            {
                var sc = new SessionContext() {SessionId = Guid.NewGuid().ToString(), UserId = userName, LastActivity = DateTime.Now}; 
                Sessions.Add(sc);
                return new LoginResponse() {SessionId = sc.SessionId};
            }
            return new LoginResponse() {ErrorMessage = "Incorrect login / password"};
        }
    }
}
