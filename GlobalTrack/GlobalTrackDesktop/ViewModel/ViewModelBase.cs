using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GlobalTrackDesktop.Annotations;

namespace GlobalTrackDesktop.ViewModel
{
   public class ViewModelBase : INotifyPropertyChanged, IChangeTracking
    {

        private bool _notifyingObjectIsChanged;
        private readonly object _notifyingObjectIsChangedSyncRoot = new Object();

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
               lock (_notifyingObjectIsChangedSyncRoot)
               {
                   return _notifyingObjectIsChanged;
               }
           }
           protected set
           {
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
