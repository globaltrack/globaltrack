//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Web;
using ClientDataModel;
using GlobalTrackService.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TrackableItemState = ServerDataModel.TrackableItemState;


namespace GlobalTrackService
{
    //[DataServiceKey("Id")]
    //[DataServiceEntity]
    //public class TrackingNew
    //{
    //    public int Id { get; set; }
    //    public string Comment { get; set; }
    //}

    [JSONPSupportBehavior]
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class GlobalTrackDataService : DataService<TrackingRestDataSource>
    {
        private static string connectionString;
        private static MongoDatabase _database;
        private static MongoCollection<ServerDataModel.Tracking> _trackingCollection;
        private static MongoCollection<ServerDataModel.TrackableItem> _trackableItemsCollection;
        private static IList<SessionContext> Sessions = new List<SessionContext>(); 


        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {

            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetServiceOperationAccessRule("GetTrackingById", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.UseVerboseErrors = true;

            connectionString = ConfigurationManager.AppSettings["mongoDbServerConnection"];
            var dbName = ConfigurationManager.AppSettings["mongoDbName"];
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(dbName);
            _database = database;
            _trackingCollection = database.GetCollection<ServerDataModel.Tracking>("Trackings");
            _trackableItemsCollection = database.GetCollection<ServerDataModel.TrackableItem>("TrackableItems");
          
        }

        protected override TrackingRestDataSource CreateDataSource()
        {
            return new TrackingRestDataSource();
        }
        
        [WebGet]
        public Tracking GetTrackingById(string trackingId)
        {
            //_trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.Id == new ObjectId())
            var tracking = _trackingCollection.AsQueryable().FirstOrDefault(t => t.Id == new ObjectId(trackingId));
            if (tracking != null)
            {
                var clientTracking = tracking.ToClientData<ClientDataModel.Tracking>();
                var trackableItem =
                    _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.Id == tracking.TrackingItemId);
                if (trackableItem != null) clientTracking.TrackingItemName = trackableItem.Name;

               var state =  trackableItem.States.FirstOrDefault(x => x.Id == tracking.StateId);
                if (state != null) clientTracking.StateName = state.Name;

                    clientTracking.History.ToList().ForEach(h =>
                    {
                        var trackableItemState = trackableItem.States.FirstOrDefault( st => st.Id == new ObjectId(h.StateId));
                        if (trackableItemState != null) h.StateName = trackableItemState.Name;
                    });
                return clientTracking;
            }
            else return null; 
        }

    }


     public class TrackingRestDataSource : IUpdatable
    {
         private static List<Tracking> _trackings = new List<Tracking>();

        // public IQueryable<Tracking> Trackings { get { return _trackings.AsQueryable(); } }
 
        static TrackingRestDataSource()
        {
            _trackings.Add(new Tracking(){Id = "1"});
            _trackings.Add(new Tracking() { Id = "2" });
            _trackings.Add(new Tracking() { Id = "3" });
 
        }
 
        #region IUpdatable Members
 
        public void ClearChanges()
        {
            throw new NotImplementedException();
        }
 
        public object CreateResource(string containerName, string fullTypeName)
        {
            Type t = Type.GetType(fullTypeName);
            if (t == typeof(Tracking))
            {
                Tracking c = new Tracking();
                _trackings.Add(c);
                return c;
            }
            else
                throw new Exception("type not found!");
        }
 
        public void DeleteResource(object targetResource)
        {
            if (targetResource.GetType() == typeof(Tracking))
            {
                _trackings.Remove(targetResource as Tracking);
            }
            else
                throw new Exception("type not found!");
        }
 
        public object GetResource(IQueryable query, string fullTypeName)
        {
            if (query is IQueryable<Tracking>)
            {
                return ((IQueryable<Tracking>)query).FirstOrDefault<Tracking>();
            }
 
            throw new Exception("type not found!");
        }
 
        public object GetValue(object targetResource, string propertyName)
        {
            System.Reflection.PropertyInfo property = targetResource.GetType().GetProperty(propertyName);
            return property.GetValue(targetResource, null);
        }
 
        public void SetValue(object targetResource, string propertyName, object propertyValue)
        {
            System.Reflection.PropertyInfo property = targetResource.GetType().GetProperty(propertyName);
            property.SetValue(targetResource, propertyValue, null);
        }
 
        public object ResetResource(object resource)
        {
            return resource;
        }
 
        public object ResolveResource(object resource)
        {
            return resource;
        }
 
        public void SaveChanges()
        {
 
        }
 
        public void SetReference(object targetResource, string propertyName, object propertyValue)
        {
            throw new NotImplementedException();
        }
        public void AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded)
        {
            throw new NotImplementedException();
        }
        public void RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
 
    
 
}

