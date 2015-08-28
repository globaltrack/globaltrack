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
using System.Windows.Shapes;
using GlobalTrackDesktop.Core;
using GlobalTrackDesktop.GlobalTrackService;

namespace GlobalTrackDesktop.UI
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog 
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void OnLogin(object sender, RoutedEventArgs e)
        {
            GlobalTrackService.GlobalTrackServicev1Client client = new GlobalTrackServicev1Client();
            var loginInfo =  client.Login(TbUser.Text.ToLower(), TbPassword.Text);
            if (string.IsNullOrEmpty(loginInfo.SessionId))
            {
                MessageBox.Show(loginInfo.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            else
            {
                UserContext.Service = client; 
                UserContext.Session = loginInfo.SessionId;
                UserContext.Password = TbPassword.Text;
                UserContext.UserName = TbUser.Text.ToLower(); 
                this.Close();
            }
        }
    }
}
