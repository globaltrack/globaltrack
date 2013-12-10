using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalTrack.Models;
using GlobalTrack.Models.ServerDataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using PagedList;
using ServerDataModel;

namespace GlobalTrack.Controllers
{
    [Authorize]
    public class TrackingController : Controller
    {
        private MongoCollection<TrackableItem> _trackableItemsCollection;
        private MongoCollection<TrackableItemState> _trackableItemStatesCollection;
        private MongoCollection<TrackingViewModel> _trackingCollection;

        private MongoDatabase _database;


        public TrackingController()
        {
            var connectionString =  ConfigurationManager.AppSettings["mongoDbServerConnection"]; 
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("db");
            _database = database;
            _trackableItemsCollection = database.GetCollection<TrackableItem>("TrackableItems"); 
            _trackableItemStatesCollection = database.GetCollection<TrackableItemState>("TrackableItemsStates");
            _trackingCollection = database.GetCollection<TrackingViewModel>("Trackings");
        }

        public ActionResult GetTrackingItemStates(ObjectId Id)
        {
            List<SelectListItem> itemStates = new List<SelectListItem>();
            //The below code is hardcoded for demo. you mat replace with DB data 
            //based on the  input coming to this method ( product id)
            var ti = _trackableItemsCollection.AsQueryable().FirstOrDefault(tii => tii.Id == Id); 
            var states = ti.States;

            if (states != null)
            {
             states.ToList().ForEach(st =>itemStates.Add(new SelectListItem()
                                                 {
                                                     Value = st.Id.ToString(),
                                                     Text = st.Name
                                                 }));
            }

            var response = new {itemStates = itemStates, supportCustomerInfo = ti.SupportsUserInformation}; 
            return Json(response, JsonRequestBehavior.AllowGet);
        } 


        //
        // GET: /Tracking/
        public ActionResult Index(int? page)
        {
            
            var query = from u in _trackingCollection.AsQueryable()
                        where u.User == User.Identity.Name
                        select u;

            var userTrackings = query.ToList();

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            ViewBag.SearchInUserInfo = true;
            ViewBag.SearchInHistory = true;
            ViewBag.SearchInTrackingNumber = true;
            return View(userTrackings.ToPagedList(pageNumber, pageSize));

        }

        [HttpPost]
        public ActionResult Index(int? page, bool? searchInUserInfo, bool? searchInHistory, bool? searchInTrackingNumber, string searchString, string startDate, string endDate)
        {

            var query = from u in _trackingCollection.AsQueryable()
                        where u.User == User.Identity.Name
                        select u;

            
            //Filtering 
            
            DateTime sDate, eDate;
            if (!string.IsNullOrWhiteSpace(startDate) && DateTime.TryParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out sDate))
            {
                query = query.Where(t => t.CreatedDate >= sDate);
            }
            if (!string.IsNullOrWhiteSpace(endDate) && DateTime.TryParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out eDate))
            {
                query = query.Where(t => t.CreatedDate <= eDate);
            }


            //Searching
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                if (searchInTrackingNumber.HasValue && searchInTrackingNumber.Value)
                {
                    ObjectId oid;
                    if (ObjectId.TryParse(searchString.Trim(), out oid))
                    {
                        query = from u in query
                                where u.Id == oid
                                select u;
                    }

                }
                if (searchInHistory.HasValue && searchInHistory.Value)
                {
                    query = from t in query
                            where (
                                      (t.CustomerInformation != null &&
                                       (t.CustomerInformation.FirstName.Contains(searchString) ||
                                        t.CustomerInformation.LastName.Contains(searchString) ||
                                        t.CustomerInformation.Email.Contains(searchString) ||
                                        t.CustomerInformation.MiddleName.Contains(searchString) ||
                                        t.CustomerInformation.Email.Contains(searchString))
                                      )
                                      ||
                                      (t.History.Any(h =>h.Comment.Contains(searchString)))
                                  )
                                select t; 
                }

            }

            var userTrackings = query.ToList();

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            ViewBag.SearchInUserInfo = true;
            ViewBag.SearchInHistory = true;
            ViewBag.SearchInTrackingNumber = true;
            return View(userTrackings.ToPagedList(pageNumber, pageSize));

        }
        //
        // GET: /Tracking/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Tracking/Create

        public ActionResult Create()
        {

            List<SelectListItem> TrackableItemsForUser =
                _trackableItemsCollection.AsQueryable()
                                         .Where(x => x.UserId == User.Identity.Name)
                                         .Select(x => new SelectListItem() {Value = x.Id.ToString(), Text = x.Name}).ToList();

            var vm = new TrackingViewModel() {Id = ObjectId.GenerateNewId(DateTime.Now), Comment = "Insert comment here", CustomerInformation = new TrackingCustomerInformation()}; 
            
           vm.TrackingItems = new SelectList(TrackableItemsForUser, "Value", "Text");
           vm.TrackingItemStates = new List<SelectListItem>();
           return View(vm);
        }

        //
        // POST: /Tracking/Create

        [HttpPost]
        public ActionResult Create(ObjectId trackingId,  Tracking tracking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var trackingItem = _trackableItemsCollection.AsQueryable().FirstOrDefault(ti => ti.Id == tracking.TrackingItemId);

                    //delete customer info if tracking item does not support it 
                    if (!trackingItem.SupportsUserInformation)
                        tracking.CustomerInformation = null;

                    tracking.CreatedDate = DateTime.Now.Date; 
                    tracking.Id = trackingId;
                    tracking.User = User.Identity.Name; 
                    tracking.History = new List<TrackingHistoryRecord>(){new TrackingHistoryRecord(){Comment = tracking.Comment, CreatedDate = DateTime.Now, StateId = tracking.StateId}} ;
                    _trackingCollection.Save(tracking);
                }
                // TODO: Add insert logic here

                return RedirectToAction("Index", "ControlPanel");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Tracking/Edit/5

        public ActionResult Edit(ObjectId id)
        {
            var tracking = _trackingCollection.AsQueryable().FirstOrDefault(t => t.Id == id);
            List<SelectListItem> trackingStates = new List<SelectListItem>();
            var trackingItem = _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.UserId == User.Identity.Name && x.Id == tracking.TrackingItemId);
            if (
                trackingItem !=
                null)
            {
                
                    trackingItem.States.ToList().ForEach( x=> trackingStates.Add(new SelectListItem(){Value = x.Id.ToString(), Text = x.Name}));
                    tracking.TrackingItemStates = trackingStates; 
            }

            
            return View(tracking);
        }

        //
        // POST: /Tracking/Edit/5

        [HttpPost]
        public ActionResult Edit(ObjectId id, Tracking tracking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingTracking = _trackingCollection.AsQueryable().FirstOrDefault(t => t.Id == id);

                    existingTracking.History.Add(new TrackingHistoryRecord()
                        {
                            Comment = tracking.Comment,
                            StateId = tracking.StateId,
                            CreatedDate = DateTime.Now
                        });
                    existingTracking.StateId = tracking.StateId;
                    existingTracking.Comment = tracking.Comment;
                    _trackingCollection.Save(existingTracking);
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Tracking/Delete/5

        public ActionResult Delete(ObjectId id)
        {
            return View();
        }

        //
        // POST: /Tracking/Delete/5

        [HttpPost]
        public ActionResult Delete(ObjectId id, Tracking tracking)
        {
            try
            {
                 _trackingCollection.Remove(Query<Tracking>.EQ(x => x.Id, id)); 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
