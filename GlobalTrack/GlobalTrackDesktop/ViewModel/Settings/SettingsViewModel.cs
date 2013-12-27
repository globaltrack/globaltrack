using System.Collections.ObjectModel;
using System.Linq;
using GlobalTrackDesktop.Core;
using MahApps.Metro;

namespace GlobalTrackDesktop.ViewModel.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        private ObservableCollection<Accent> _colors;
        private Accent _color;

        public SettingsViewModel()
        {
            Colors = new ObservableCollection<Accent>();
            ThemeManager.DefaultAccents.ToList().ForEach(x => Colors.Add(x));
            Color = ThemeManager.DefaultAccents.FirstOrDefault(x => x.Name == UserContext.Color); 
        }


        public ObservableCollection<Accent> Colors
        {
            get { return _colors; }
            set { _colors = value; OnPropertyChanged("Colors");}
        }

        public Accent Color
        {
            get { return _color; }
            set { 
                _color = value; 
                OnPropertyChanged("Color");
                if (_color != null)
                    UserContext.Color = value.Name; 
            }
        }

    }
}
