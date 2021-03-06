﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GlobalTrackDesktop.ViewModel.TrackableItems;

namespace GlobalTrackDesktop.UI.TrackableItems
{
    /// <summary>
    /// Interaction logic for TrackableItemStateDialog.xaml
    /// </summary>
    public partial class TrackableItemStateDialog
    {
        public TrackableItemStateDialog(TrackableItemStateViewModel vm)
        {
            InitializeComponent();
            DataContext = vm; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult =  false; 
            this.Close();
        }
    }
}
