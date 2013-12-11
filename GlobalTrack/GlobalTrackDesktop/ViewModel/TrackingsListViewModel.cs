using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ClientDataModel;
using GlobalTrackDesktop.Core;
using GlobalTrackDesktop.GlobalTrackService;
using GlobalTrackDesktop.UI;

namespace GlobalTrackDesktop.ViewModel
{
    public class TrackingsListViewModel : ViewModelBase
    {
        private GlobalTrackService.IGlobalTrackServicev1 service;


        public ICommand ShowHistoryCommand { get; set; }

        public TrackingsListViewModel()
        {
            ShowHistoryCommand = new DelegateCommand(ShowTrackingHistory, x => true);
            service = UserContext.Service;
            try
            {
                IList<ClientDataModel.Tracking> items = service.GetTrackings(UserContext.Session, null, null, "");
                Trackings = new ObservableCollection<Tracking>();

                foreach (var item in items)
                    Trackings.Add(item);
            }
            catch (Exception)
            {
                throw; 

            }
            
        }

        private void ShowTrackingHistory(object obj)
        {
            TrackingHistoryDialog dlg = new TrackingHistoryDialog( new TrackingHistoryViewModel( obj as Tracking));
            dlg.ShowDialog();

        }

        private ObservableCollection<Tracking> _trackings;

        public ObservableCollection<Tracking> Trackings
        {
            get { return _trackings; }
            set { _trackings = value;  OnPropertyChanged("Trackings");}
        }
    }
}
