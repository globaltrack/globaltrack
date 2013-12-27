using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientDataModel;

namespace GlobalTrackDesktop.ViewModel.TrackableItems
{
    public class TrackableItemStateViewModel : ViewModelBase
    {
        public TrackableItemState State { get; set; }
    

        public TrackableItemStateViewModel(TrackableItemState trackableItemState)
        {
            State = trackableItemState; 

        }
    }
}
