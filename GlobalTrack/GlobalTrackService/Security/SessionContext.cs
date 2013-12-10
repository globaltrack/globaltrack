using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalTrackService.Security
{
    public class SessionContext
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public DateTime LastActivity { get; set; }

    }
}