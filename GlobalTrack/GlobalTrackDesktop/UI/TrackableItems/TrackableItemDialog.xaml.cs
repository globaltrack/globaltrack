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
    }

   
}
