using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GlobalTrackDesktop.Core;

namespace GlobalTrackDesktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public TrackingsListViewModel TrackingsVm { get; set; }

        public TrackableItemsViewModel TrackableItemsVm { get; set; }

        public SettingsViewModel SettingsVm { get; set; }


        public ICommand CreateNewTrackingCommand { get; set; }

        public MainViewModel()
        {
            CreateNewTrackingCommand = new DelegateCommand(CreateNewTracking, x => true);
            TrackingsVm = new TrackingsListViewModel();
            TrackableItemsVm = new TrackableItemsViewModel();
            SettingsVm = new SettingsViewModel();
        }

        private void CreateNewTracking(object obj)
        {
            MessageBox.Show("Create new tracking executed"); 
        }
    }
}
