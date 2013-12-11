﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ClientDataModel
{
    [DataContract]
    public class TrackableItemState
    {
        [DataMember]
        public string Id { get; set; }
        
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string Description { get; set; }

    }
}
