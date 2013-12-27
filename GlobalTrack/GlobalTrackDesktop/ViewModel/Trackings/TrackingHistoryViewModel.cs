using ClientDataModel;

namespace GlobalTrackDesktop.ViewModel.Trackings
{
    public class TrackingHistoryViewModel : ViewModelBase
    {
        public TrackingHistoryViewModel(Tracking t)
        {
            this.Tracking = t; 
            
        }
        
        public ClientDataModel.Tracking Tracking { get; set; }
    


    }
}
