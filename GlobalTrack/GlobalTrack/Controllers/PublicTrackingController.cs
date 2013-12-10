using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalTrack.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServerDataModel;

namespace GlobalTrack.Controllers
{
    public class PublicTrackingController : Controller
    {
        private MongoCollection<TrackableItem> _trackableItemsCollection;
        private MongoCollection<TrackableItemState> _trackableItemStatesCollection;
        private MongoCollection<Tracking> _trackingCollection;

        private MongoDatabase _database;


        public PublicTrackingController()
        {
            var connectionString = ConfigurationManager.AppSettings["mongoDbServerConnection"]; 
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("db");
            _database = database;
            _trackableItemsCollection = database.GetCollection<TrackableItem>("TrackableItems"); 
            _trackableItemStatesCollection = database.GetCollection<TrackableItemState>("TrackableItemsStates");
            _trackingCollection = database.GetCollection<Tracking>("Trackings");
        }
        
        //
        // GET: /PublicTracking/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(SearchTracking st)
        {
            try
            {
                ObjectId id;
                if (ObjectId.TryParse(st.TrackingNumber, out id))
                {
                    //try ti search in db 
                    Tracking t = _trackingCollection.AsQueryable().FirstOrDefault(x => x.Id == id);
                    if (t != null)
                    {
                        //found 
                        return RedirectToAction("TrackingDetails", new {trackingId = st.TrackingNumber}); 

                    }
                    else
                    {
                        ModelState.AddModelError("", "The item with speficied number wasn't found in our system. Please verify your input.");
                    }
                }
                else
                {
                    ModelState.AddModelError("TrackingNumber", "Incorrect tracking number. Please verify your input.");
                    //incorrect tracking number 
                }
            }
            catch
            {
                return View(); 
            }
            return View();
        }

        public ActionResult TrackingDetails(string trackingId)
        {
            ObjectId id = ObjectId.Parse(trackingId);

            Tracking t = _trackingCollection.AsQueryable().FirstOrDefault(x => x.Id == id);
            TrackableItem ti = _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.Id == t.TrackingItemId);
            TrackableItemState currentState = ti.States.FirstOrDefault(state => state.Id == t.StateId);

            ViewBag.StateNames = new Dictionary<string, string>();
            ti.States.ToList().ForEach(s => ((Dictionary<string, string>)ViewBag.StateNames).Add(s.Id.ToString(), s.Name)); 


            SearchTrackingInfo si = new SearchTrackingInfo();
            si.TrackingName = t.Id.ToString();
            si.TrackabeItemName = ti.Name;
            si.State = currentState.Name;
            si.History = t.History;
            si.SupportsGeolocationServices = ti.SupportsGeolocationServices; 

            return View(si); 
        }
    }
}
