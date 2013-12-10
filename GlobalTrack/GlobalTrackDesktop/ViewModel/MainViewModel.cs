﻿using System;
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

        public ICommand CreateNewTrackingCommand { get; set; }

        public MainViewModel()
        {
            CreateNewTrackingCommand = new DelegateCommand(CreateNewTracking, x => true);
            TrackingsVm = new TrackingsListViewModel();
        }

        private void CreateNewTracking(object obj)
        {
            MessageBox.Show("Create new tracking executed"); 
        }
    }
}
