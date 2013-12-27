using GlobalTrackDesktop.Core;
using GlobalTrackDesktop.ViewModel.TrackableItems;

namespace GlobalTrackDesktop.UI.TrackableItems
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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UserContext.Service.UpdateTrackableItem(UserContext.Session,
                                                    (DataContext as TrackableItemViewModel).DataSource); 
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }

   
}
