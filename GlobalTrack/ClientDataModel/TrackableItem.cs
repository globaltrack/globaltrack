using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientDataModel
{
    public class TrackableItem
    {
        public string UserId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsSecured { get; set; }


        public bool SupportsGeolocationServices { get; set; }


        public bool SupportsUserInformation { get; set; }

        public IList<TrackableItemState> States { get; set; }
    }
}
