using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ClientDataModel;
using GlobalTrackDesktop.Core;
using GlobalTrackDesktop.GlobalTrackService;
using GlobalTrackDesktop.UI;

namespace GlobalTrackDesktop.ViewModel
{
    public class TrackableItemsViewModel : ViewModelBase
    {

        private GlobalTrackService.IGlobalTrackServicev1 service;

        private ObservableCollection<TrackableItem> _trackableItems; 

        public ICommand EditStatesCommand { get; set; }

        public ObservableCollection<TrackableItem> TrackableItems
        {
            get { return _trackableItems; }
            set { _trackableItems = value; OnPropertyChanged("TrackableItems"); }
        }

        public TrackableItemsViewModel()
        {
            EditStatesCommand = new DelegateCommand(EditStates, x => true);

            service = UserContext.Service;
            try
            {
                TrackableItems = new ObservableCollection<TrackableItem>();
                IList<TrackableItem> items = service.GetTrackableItems(UserContext.Session);
                
                //initially accepting all changes at all items 
                //TODO: think about 'how to automate this process'? 
                items.ToList().ForEach(x =>
                    {
                        x.AcceptChanges();
                        TrackableItems.Add(x);
                    });
                
            }
            catch (Exception)
            {
                throw;

            }
        }

        private void EditStates(object obj)
        {
            TrackableItemDialog dlg = new TrackableItemDialog( new TrackableItemViewModel(obj as TrackableItem));
            dlg.ShowDialog(); 
        }
    }
}
