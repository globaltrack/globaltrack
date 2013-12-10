using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GlobalTrack.Controllers
{
    [Authorize]
    public class ControlPanelController : Controller
    {
        //
        // GET: /ControlPanel/
        public ActionResult Index()
        {
            ViewBag.UserName = this.User.Identity.Name;
            ViewBag.UserId = User.Identity;
            MembershipUser u = Membership.GetUser();
            
            return View();
        }

    }
}
