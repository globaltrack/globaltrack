using System.Collections.Generic;
using System.ComponentModel;

namespace ServerDataModel
{
    public class SearchTrackingInfo
    {
        [DisplayName("Tracking number")]
        public string TrackingName { get; set; }
        [DisplayName("Tracking item")]
        public string TrackabeItemName { get; set;}
        [DisplayName("Current status")]
        public string State { get; set; }
        public IList<TrackableItemState> PossibleStates { get; set; }
        public IList<TrackingHistoryRecord> History { get; set; }
        public bool SupportsGeolocationServices { get; set; }
    }
}