using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GlobalTrackDesktop.Core;
using GlobalTrackDesktop.UI;
using GlobalTrackDesktop.ViewModel;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace GlobalTrackDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            
            UserContext.Initialize();
            InitializeComponent();
            //ThemeManager.ChangeTheme(Application.Current, ThemeManager.DefaultAccents.First(x => x.Name == "Brown"), Theme.Light);
            LoginDialog dlg = new LoginDialog();
            dlg.ShowDialog();
            DataContext = new MainViewModel();
            //SettingsViewControl.DataContext = new SettingsViewModel(); 
            

        }
    }
}
