using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalTrackDesktop.Core
{
    static public class UserContext
    {
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string Session { get; set; }
        public static GlobalTrackService.IGlobalTrackServicev1 Service { get; set; }
 

    }
}
