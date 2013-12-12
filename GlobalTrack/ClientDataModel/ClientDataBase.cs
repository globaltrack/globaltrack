using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ClientDataModel.Annotations;

namespace ClientDataModel
{
    [DataContract]
    public abstract class ClientDataBase : IChangeTracking, INotifyPropertyChanged
    {
        private bool _notifyingObjectIsChanged;
        private object _notifyingObjectIsChangedSyncRoot = new Object();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (!String.Equals(propertyName, "IsChanged", StringComparison.Ordinal))
            {
                this.IsChanged = true;
            }
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AcceptChanges()
        {
            this.IsChanged = false;
        }

        public bool IsChanged
        {
            get
            {
                if (_notifyingObjectIsChangedSyncRoot == null)
                    _notifyingObjectIsChangedSyncRoot = new Object();
                lock (_notifyingObjectIsChangedSyncRoot)
                {
                    return _notifyingObjectIsChanged;
                }
            }
            protected set
            
            {
                if (_notifyingObjectIsChangedSyncRoot == null)
                    _notifyingObjectIsChangedSyncRoot = new Object();

                lock (_notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(_notifyingObjectIsChanged, value))
                    {
                        _notifyingObjectIsChanged = value;

                        this.OnPropertyChanged("IsChanged");
                    }
                }

            }
        }
    }
}
