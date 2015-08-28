using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GlobalTrack.Filters;
using GlobalTrack.Models.Security;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServerDataModel;

namespace GlobalTrack.Controllers
{
    [Authorize]
    [Culture]
    public class ControlPanelController : Controller
    {
        private MongoCollection<User> _usersCollection;
        private MongoDatabase _database;

        public ControlPanelController()
        {
            var connectionString = ConfigurationManager.AppSettings["mongoDbServerConnection"];
            var dbName = ConfigurationManager.AppSettings["mongoDbName"]; 
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(dbName);
            _database = database;
            _usersCollection = database.GetCollection<User>("Users"); 
        }
        //
        // GET: /ControlPanel/
        public ActionResult Index()
        {
            ViewBag.UserName = this.User.Identity.Name;
            ViewBag.UserId = User.Identity;
            MembershipUser u = Membership.GetUser();
            
            return View();
        }

        // GET: 
        public ActionResult UserInformation()
        {
            var username = Membership.GetUser().UserName.ToLower();
            var user =
                _usersCollection.AsQueryable().FirstOrDefault(x => string.Equals(x.NameLowerSpace, username));
            return View(user); 
        }
        // POST: 
        [HttpPost]
        public ActionResult UserInformation(User user)
        {
            try
            {
                _usersCollection.Save(user); 
            
            }
            catch (Exception)
            {

                return View(user);
            }
            return RedirectToAction("Index"); 
            
        }
    }
}
