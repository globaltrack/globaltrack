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
using GlobalTrackDesktop.ViewModel;

namespace GlobalTrackDesktop.UI
{
    /// <summary>
    /// Interaction logic for TrackableItemDialog.xaml
    /// </summary>
    public partial class TrackableItemDialog 
    {
        public TrackableItemDialog(TrackableItemViewModel vm )
        {
            InitializeComponent();
            DataContext = vm;
            Title = string.Format("Trackable Item: {0}", vm.DataSource.Name); 
        }
    }

   
}
