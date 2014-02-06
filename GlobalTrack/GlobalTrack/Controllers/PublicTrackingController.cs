using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalTrack.Filters;
using GlobalTrack.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Resources;
using ServerDataModel;

namespace GlobalTrack.Controllers
{
    [Culture]
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
            ViewBag.PasswordRequired = false; 
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
                        if (string.IsNullOrEmpty(t.Password) ||
                            (!string.IsNullOrWhiteSpace(st.Password) && !string.IsNullOrEmpty(t.Password) &&
                             string.Equals(st.Password, t.Password)))
                        {
                            //found and unsecured or password is OK: 

                            SearchTrackingInfo si = new SearchTrackingInfo();
                            si.QrData = QRCodeHtmlHelper.CreateQrData(t.Id.ToString(), t.Password); 
                            TrackableItem ti =
                                _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.Id == t.TrackingItemId);
                            TrackableItemState currentState = ti.States.FirstOrDefault(state => state.Id == t.StateId);
                            si.TrackingName = t.Id.ToString();
                            si.TrackabeItemName = ti.Name;
                            si.State = currentState.Name;
                            si.History = t.History;
                            si.SupportsGeolocationServices = ti.SupportsGeolocationServices;
                            ViewBag.StateNames = new Dictionary<string, string>();
                            ti.States.ToList()
                              .ForEach(
                                  s => ((Dictionary<string, string>) ViewBag.StateNames).Add(s.Id.ToString(), s.Name));
                            return View("TrackingDetails", si);
                            //return RedirectToAction("TrackingDetails", new {trackingId = st.TrackingNumber});
                        }
                        else
                        {
                            //need password
                            ViewBag.PasswordRequired = true;
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", Resource.PublicTrackingController_Index_The_item_with_speficied_number_wasn_t_found_in_our_system__Please_verify_your_input_);
                    }
                }
                else
                {
                    ModelState.AddModelError("TrackingNumber", Resource.PublicTrackingController_Index_Incorrect_tracking_number__Please_verify_your_input_);
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
            try
            {
                ObjectId id = ObjectId.Parse(trackingId);

                Tracking t = _trackingCollection.AsQueryable().FirstOrDefault(x => x.Id == id);
                TrackableItem ti = _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.Id == t.TrackingItemId);
                TrackableItemState currentState = ti.States.FirstOrDefault(state => state.Id == t.StateId);

                ViewBag.StateNames = new Dictionary<string, string>();
                ti.States.ToList().ForEach(s => ((Dictionary<string, string>)ViewBag.StateNames).Add(s.Id.ToString(), s.Name));


                SearchTrackingInfo si = new SearchTrackingInfo();
                si.QrData = QRCodeHtmlHelper.CreateQrData(t.Id.ToString(), t.Password); 
                si.TrackingName = t.Id.ToString();
                si.TrackabeItemName = ti.Name;
                si.State = currentState.Name;
                si.History = t.History;
                si.SupportsGeolocationServices = ti.SupportsGeolocationServices;

                return View(si); 
            }
            catch (Exception)
            {
                return new HttpNotFoundResult("The tracking with a specified tracking number doesn't exist");
                
            }
            
        }
    }
}
