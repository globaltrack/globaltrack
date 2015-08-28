using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using GlobalTrack.Code;
using GlobalTrack.Filters;
using GlobalTrack.Models;
using GlobalTrack.Models.Security;
using GlobalTrack.Models.ServerDataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using PagedList;
using ServerDataModel;
using Utilities;

namespace GlobalTrack.Controllers
{
    [Authorize]
    [Culture]
    public class TrackingController : Controller
    {
        private MongoCollection<TrackableItem> _trackableItemsCollection;
        private MongoCollection<TrackableItemState> _trackableItemStatesCollection;
        private MongoCollection<TrackingViewModel> _trackingCollection;
        private MongoCollection<User> _usersCollection;


        private MongoDatabase _database;


        public TrackingController()
        {
            var connectionString =  ConfigurationManager.AppSettings["mongoDbServerConnection"];
            var dbName = ConfigurationManager.AppSettings["mongoDbName"]; 
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(dbName);
            _database = database;
            _trackableItemsCollection = database.GetCollection<TrackableItem>("TrackableItems"); 
            _trackableItemStatesCollection = database.GetCollection<TrackableItemState>("TrackableItemsStates");
            _trackingCollection = database.GetCollection<TrackingViewModel>("Trackings");
            _usersCollection = database.GetCollection<User>("Users"); 

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
            
            //var query = from u in _trackingCollection.AsQueryable()
            //            where u.User == User.Identity.Name  
                        
            //            select u;

            var query =
                _trackingCollection.AsQueryable().Where(t => t.User == User.Identity.Name).ToList();
            
            //apply tracking item name for each tracking 
            query.ForEach(
                t =>
                    {
                        var firstOrDefault  = _trackableItemsCollection.AsQueryable().FirstOrDefault(ti => ti.Id == t.TrackingItemId);
                        if (firstOrDefault !=
                            null)
                            t.TrackingItemName =
                                firstOrDefault.Name;
                    }); 
                        

            var userTrackings = query.ToList();

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            ViewBag.SearchInUserInfo = true;
            ViewBag.SearchInHistory = true;
            ViewBag.SearchInTrackingNumber = true;
            
            //combobox with trackable items filter 
            ViewBag.TrackingItemId = string.Empty;
            List<SelectListItem> trackableItemsForUser =
               _trackableItemsCollection.AsQueryable()
                                        .Where(x => x.UserId == User.Identity.Name)
                                        .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();

           // var vm = new TrackingViewModel() { Id = ObjectId.GenerateNewId(DateTime.Now), Comment = "Insert comment here", CustomerInformation = new TrackingCustomerInformation() };
            trackableItemsForUser.Insert(0, new SelectListItem(){Text = "All", Value = string.Empty, Selected = true});
            ViewBag.TrackingItems = new SelectList(trackableItemsForUser, "Value", "Text");

            return View(userTrackings.ToPagedList(pageNumber, pageSize));

        }

        [HttpPost]
        public ActionResult Index(int? page, bool? searchInUserInfo, bool? searchInHistory, bool? searchInTrackingNumber, string searchString, string startDate, string endDate, string selectedTrackableItem)
        {
            //combobox with trackable items filter 
            ViewBag.TrackingItemId = string.Empty;
            List<SelectListItem> trackableItemsForUser =
               _trackableItemsCollection.AsQueryable()
                                        .Where(x => x.UserId == User.Identity.Name)
                                        .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();

            // var vm = new TrackingViewModel() { Id = ObjectId.GenerateNewId(DateTime.Now), Comment = "Insert comment here", CustomerInformation = new TrackingCustomerInformation() };
            trackableItemsForUser.Insert(0, new SelectListItem() { Text = "All", Value = string.Empty});
            ViewBag.TrackingItems = new SelectList(trackableItemsForUser, "Value", "Text");

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

            ObjectId specificTrackableItemId;
            if (!string.IsNullOrEmpty(selectedTrackableItem) &&
                ObjectId.TryParse(selectedTrackableItem, out specificTrackableItemId))
            {
                query = query.Where(t => t.TrackingItemId == specificTrackableItemId); 
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

            List<TrackingViewModel> queryList = query.ToList(); 
            //apply tracking item name for each tracking 
            queryList.ForEach(
                t =>
                {
                    var firstOrDefault = _trackableItemsCollection.AsQueryable().FirstOrDefault(ti => ti.Id == t.TrackingItemId);
                    if (firstOrDefault !=
                        null)
                        t.TrackingItemName =
                            firstOrDefault.Name;
                }); 

            var userTrackings = queryList;

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
                    
                    tracking.Password = trackingItem.IsSecured ? PasswordGenerator.CreateRandomPassword(8) : string.Empty; 
                    _trackingCollection.Save(tracking);
                }
                try
                {
                    var username = Membership.GetUser().UserName.ToLower();
                    var user =
                        _usersCollection.AsQueryable().FirstOrDefault(x => string.Equals(x.NameLowerSpace, username));
                    if (user.EnableSubscribersEmailNotifications)
                    {
                        if (tracking.CustomerInformation != null && !string.IsNullOrWhiteSpace(tracking.CustomerInformation.Email))
                        {
                            var trackingItem = _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.UserId == User.Identity.Name && x.Id == tracking.TrackingItemId);
                            var stateName = trackingItem.States.FirstOrDefault(s => s.Id == tracking.StateId).Name;
                            SubscriberMailSettings emailSettings = new SubscriberMailSettings();
                            emailSettings.SourceEmailAddress = user.SmtpUserName;
                            emailSettings.SmtpPassword = user.SmtpPassword;
                            emailSettings.SmtpPort = user.SmtpPort;
                            emailSettings.SmtpUrl = user.SmtpServerUrl;
                            emailSettings.UseSsl = user.SmtpHttps;
                            emailSettings.DestinationEmailAddress = tracking.CustomerInformation.Email;
                            emailSettings.SourceFullName = string.Format("{0} {1}", user.FirstName, user.LastName);
                            emailSettings.DestinationFullName = string.Format("{0} {1}", tracking.CustomerInformation.FirstName, tracking.CustomerInformation.LastName);
                            var trackingData = QRCodeHtmlHelper.CreateQrData(tracking.TrackingNumber, tracking.Password);
                            
                            var qrUrl = QRCodeHtmlHelper.QRCode(null, trackingData, 150);

                            Mailer.NotifyNewTracking(emailSettings, tracking.TrackingNumber, stateName, tracking.Comment, trackingItem.Name, qrUrl.ToString(), tracking.Password);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["errorsOccured"] = string.Format("Unable to send a notification email: {0}", ex.Message); 

                }
                return RedirectToAction("PrintableVersion", "Tracking", new { id = trackingId });
            }
            catch
            {
                return View();
            }
        }

        private void UpdateTrackingViewModel(TrackingViewModel tm)
        {
            List<SelectListItem> trackingStates = new List<SelectListItem>();
            var trackingItem = _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.UserId == User.Identity.Name && x.Id == tm.TrackingItemId);
            if (
                trackingItem !=
                null)
            {

                trackingItem.States.ToList().ForEach(x => trackingStates.Add(new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }));
                tm.TrackingItemStates = trackingStates;
            }
            

        }


        public ActionResult Edit(ObjectId id)
        {
            var tracking = _trackingCollection.AsQueryable().FirstOrDefault(t => t.Id == id);
            UpdateTrackingViewModel(tracking); 
            
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

                    ////try to send email if supported 
                    //try
                    //{
                    //    var username = Membership.GetUser().UserName.ToLower();
                    //    var user =
                    //        _usersCollection.AsQueryable().FirstOrDefault(x => string.Equals(x.NameLowerSpace, username));
                    //    if (user.EnableSubscribersEmailNotifications)
                    //    {
                    //        if (existingTracking.CustomerInformation != null && !string.IsNullOrWhiteSpace(existingTracking.CustomerInformation.Email))
                    //        {
                    //            var trackingItem = _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.UserId == User.Identity.Name && x.Id == existingTracking.TrackingItemId);
                    //            var stateName = trackingItem.States.FirstOrDefault(s => s.Id == tracking.StateId).Name;
                    //            SubscriberMailSettings emailSettings = new SubscriberMailSettings();
                    //            emailSettings.SourceEmailAddress = user.SmtpUserName;
                    //            emailSettings.SmtpPassword = user.SmtpPassword;
                    //            emailSettings.SmtpPort = user.SmtpPort;
                    //            emailSettings.SmtpUrl = user.SmtpServerUrl;
                    //            emailSettings.UseSsl = user.SmtpHttps;
                    //            emailSettings.DestinationEmailAddress = existingTracking.CustomerInformation.Email;
                    //            emailSettings.SourceFullName = string.Format("{0} {1}", user.FirstName, user.LastName);
                    //            emailSettings.DestinationFullName = string.Format("{0} {1}", existingTracking.CustomerInformation.FirstName, existingTracking.CustomerInformation.LastName);

                    //            Mailer.NotifyTrackingStatusChanged(emailSettings, existingTracking.TrackingNumber, stateName, tracking.Comment);
                    //        }
                    //    }
                    //}
                    //catch (Exception e)
                    //{
                    //    TempData["errorsOccurred"] = string.Format("System failed to send email notification with your SMTP settings.\n{0}", e.Message); 
                    //}
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(tracking as TrackingViewModel);   
                }
            }
            catch
            {
                return View(tracking as TrackingViewModel);
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


        public ActionResult PrintableVersion(ObjectId id)
        {
            var tracking = _trackingCollection.AsQueryable().FirstOrDefault(t => t.Id == id);
            List<SelectListItem> trackingStates = new List<SelectListItem>();
            var trackingItem = _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.UserId == User.Identity.Name && x.Id == tracking.TrackingItemId);
            if (
                trackingItem !=
                null)
            {

                trackingItem.States.ToList().ForEach(x => trackingStates.Add(new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }));
                tracking.TrackingItemStates = trackingStates;
            }


            return View(tracking);
        }
    }
}
