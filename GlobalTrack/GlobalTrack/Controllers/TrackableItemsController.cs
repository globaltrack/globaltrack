using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalTrack.Filters;
using GlobalTrack.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using ServerDataModel;

namespace GlobalTrack.Controllers
{

    [Authorize]
    [Culture]
    public class TrackableItemsController : Controller
    {
        private MongoCollection<TrackableItem> _trackableItemsCollection;
        private MongoCollection<TrackableItemState> _trackableItemStatesCollection;
        private MongoDatabase _database; 

        
        public TrackableItemsController()
        {
            var connectionString =  ConfigurationManager.AppSettings["mongoDbServerConnection"]; 
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("db");
            _database = database;
            _trackableItemsCollection = database.GetCollection<TrackableItem>("TrackableItems"); 
            _trackableItemStatesCollection = database.GetCollection<TrackableItemState>("TrackableItemsStates");
        }

        //
        // GET: /TrackableItems/
        public ActionResult Index()
        {
            var query = Query<TrackableItem>.Where(x => x.UserId == User.Identity.Name);
            return View(_trackableItemsCollection.Find(query));
        }

        //
        // GET: /TrackableItems/Details/5
        public ActionResult Details(ObjectId id)
        {
            
            var item = _trackableItemsCollection.FindOne(Query<TrackableItem>.EQ(x => x.Id, id)); 
            if (item.States == null) item.States = new List<TrackableItemState>();
            return View(item);
        }

        //
        // GET: /TrackableItems/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TrackableItems/Create
        [HttpPost]
        public ActionResult Create(TrackableItem ti)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ti.UserId = User.Identity.Name; 
                    _trackableItemsCollection.Insert(ti);
                }
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: /TrackableItems/CreateCreateState
        public ActionResult CreateState(ObjectId id)
        {
            
            //id is a trackable item's id 
            
            var ti = _trackableItemsCollection.AsQueryable<TrackableItem>().First(x => x.Id == id); 
            
            ViewBag.TrackableItemId = id;
            ViewBag.TrackableItem = ti.Name; 
            
            return View();
        }
        // GET: /TrackableItems/CreateCreateState
        [HttpPost]
        public ActionResult CreateState(ObjectId trackableItemId, TrackableItemState state)
        {
            if (ModelState.IsValid)
            {
                state.Id = ObjectId.GenerateNewId(); 
                var tItem = _trackableItemsCollection.Find(Query<TrackableItem>.EQ(x=> x.Id, trackableItemId)).First(); 
                if (tItem.States == null) tItem.States = new List<TrackableItemState>();
                tItem.States.Add(state);
                _trackableItemsCollection.Save(tItem); 
               
                return RedirectToAction("Details", new { id = trackableItemId });
            }

            return View();
        }


        //
        // GET: /TrackableItems/Edit/5
        public ActionResult Edit(ObjectId id)
        {
            var item = _trackableItemsCollection.FindOne(Query<TrackableItem>.EQ(x => x.Id, id));
            return View(item);
        }

        //
        // POST: /TrackableItems/Edit/5
        [HttpPost]
        public ActionResult Edit(ObjectId id, TrackableItem ti)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var oldItem = _trackableItemsCollection.AsQueryable().FirstOrDefault(x => x.Id == id);
                    ti.States = oldItem.States; 
                    _trackableItemsCollection.Save(ti);
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
        // GET: /TrackableItems/Delete/5
        public ActionResult Delete(ObjectId id)
        {
            
            var item = _trackableItemsCollection.FindOne(Query<TrackableItem>.EQ(x => x.Id, id));
            return View(item);
        }

        //
        // POST: /TrackableItems/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(ObjectId id, TrackableItem ti)
        {
            try
            {
               
                var item = _trackableItemsCollection.Remove(Query<TrackableItem>.EQ(x => x.Id, id));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /TrackableItems/Edit/5
        public ActionResult EditState(ObjectId trackableItemId, ObjectId stateId)
        {
            var state = _trackableItemsCollection.FindOne(Query<TrackableItem>.EQ(x => x.Id, trackableItemId)).States.Where(x=> x.Id == stateId).FirstOrDefault();
            ViewBag.TrackableItemId = trackableItemId;
            ViewBag.StateId = stateId; 
            return View(state);
        }

        //
        // POST: /TrackableItems/Edit/5
        [HttpPost]
        public ActionResult EditState(ObjectId trackableItemId, ObjectId stateId, TrackableItemState state)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    //updating stateid - we lost it
                    state.Id = stateId;
                    var item = _trackableItemsCollection.AsQueryable().First(x => x.Id == trackableItemId); 
                    
                    int indexOfState = item.States.Where(s=> s.Id == stateId).Select(item.States.IndexOf).FirstOrDefault();
                    item.States[indexOfState] = state;

                    _trackableItemsCollection.Save(item); 
                    
                    return RedirectToAction("Details", new {id =  trackableItemId.ToString()});
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

    }
}
