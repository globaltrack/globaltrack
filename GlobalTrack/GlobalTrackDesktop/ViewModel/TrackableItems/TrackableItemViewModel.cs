using System.Windows.Input;
using ClientDataModel;
using GlobalTrackDesktop.Core;
using GlobalTrackDesktop.UI.TrackableItems;

namespace GlobalTrackDesktop.ViewModel.TrackableItems
{
    public class TrackableItemViewModel : ViewModelBase
    {
        private TrackableItem _dataSource;

        public TrackableItemViewModel(TrackableItem ti)
        {
            _dataSource = ti;
            AddStateCommand = new DelegateCommand(AddState, x => true);
            EditStateCommand = new DelegateCommand(EditState, x => _selectedState != null);
            RemoveStateCommand = new DelegateCommand(RemoveState, x => _selectedState != null);
        }

        private TrackableItemState _selectedState;

        public TrackableItem DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; OnPropertyChanged("DataSource"); }
        }

        public ICommand AddStateCommand { get; set; }
        public ICommand EditStateCommand { get; set; }
        public ICommand RemoveStateCommand { get; set; }

        public TrackableItemState SelectedState
        {
            get { return _selectedState; }
            set { 
                _selectedState = value;
                var delegateCommand = EditStateCommand as DelegateCommand;
                if (delegateCommand != null)
                    delegateCommand.RaiseCanExecuteChanged();
                var command = RemoveStateCommand as DelegateCommand;
                if (command != null)
                    command.RaiseCanExecuteChanged();
            }
        }

        private void RemoveState(object obj)
        {
            
        }

        private void EditState(object obj)
        {
            var dlg = new TrackableItemStateDialog(new TrackableItemStateViewModel(SelectedState));
            dlg.ShowDialog(); 

        }

        private void AddState(object obj)
        {
            var dlg = new TrackableItemStateDialog(new TrackableItemStateViewModel(new TrackableItemState()));
            dlg.ShowDialog(); 

        }
    }
}
