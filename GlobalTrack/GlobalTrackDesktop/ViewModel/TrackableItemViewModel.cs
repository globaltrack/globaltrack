using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ClientDataModel;

namespace GlobalTrackDesktop.ViewModel
{
    public class TrackableItemViewModel : ViewModelBase
    {
        private TrackableItem _dataSource;

        public TrackableItemViewModel(TrackableItem ti)
        {
            _dataSource = ti; 

        }


        public TrackableItem DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; OnPropertyChanged("DataSource");}
        }

        
    }
}
