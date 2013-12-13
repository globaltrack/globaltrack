using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using MahApps.Metro;

namespace GlobalTrackDesktop.Core
{
    static public class UserContext
    {
        
        public static void Initialize()
        {
            Color = Properties.Settings.Default.Color; 
        }

        private static string _color;
 
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string Session { get; set; }
        public static string Color {
            get { return _color;  }
            set
            {
                _color = value;
                Properties.Settings.Default.Color = value;  
                Properties.Settings.Default.Save();
                ThemeManager.ChangeTheme(Application.Current, ThemeManager.DefaultAccents.FirstOrDefault(x => x.Name == value), Theme.Light);

            }
            
        }
        public static GlobalTrackService.IGlobalTrackServicev1 Service { get; set; }
 

    }
}
