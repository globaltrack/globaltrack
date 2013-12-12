using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ClientDataModel
{
    [DataContract]
    public class TrackableItemState : ClientDataBase, IDataErrorInfo
    {
        [DataMember]
        public string Id { get; set; }
        
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string Description { get; set; }

        public string this[string columnName]
        {
            get
            {
                var errMessage = string.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                        {
                            errMessage = "Name field must be provided";
                        }
                        break;
                    case "Description":
                        if (string.IsNullOrWhiteSpace(Description))
                        {
                            errMessage = "Description field must be provided";
                        }
                        break;

                    default:
                        errMessage = string.Empty;
                        break;
                }
                return errMessage;
            }
        }

        public string Error { get; private set; }
    }
}
