using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientDataModel;

namespace GlobalTrackDesktop.ViewModel
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
