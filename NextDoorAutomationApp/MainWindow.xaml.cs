using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace NextDoorAutomationApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region truong
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        private bool _isGologinInfoActive;
        public bool IsGologinInfoActive
        {
            get { return _isGologinInfoActive; }
            set
            {
                _isGologinInfoActive = value;
                OnPropertyChanged(nameof(IsGologinInfoActive));
            }
        }

        private bool _isProfileInfoActive;
        public bool IsProfileInfoActive
        {
            get { return _isProfileInfoActive; }
            set
            {
                _isProfileInfoActive = value;
                OnPropertyChanged(nameof(IsProfileInfoActive));
            }
        }

        private bool _isPostInfoActive;
        public bool IsPostInfoActive
        {
            get { return _isPostInfoActive; }
            set
            {
                _isPostInfoActive = value;
                OnPropertyChanged(nameof(IsPostInfoActive));
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            IsGologinInfoActive = false;
            IsProfileInfoActive = false;
            IsPostInfoActive = true;
            CurrentView = new PostView();

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region func
        private void GologinInfo_Click(object sender, RoutedEventArgs e)
        {
            IsGologinInfoActive = true;
            IsProfileInfoActive = false;
            IsPostInfoActive = false;
            CurrentView = new GologinView();
        }

        private void ProfileInfo_Click(object sender, RoutedEventArgs e)
        {
            IsGologinInfoActive = false;
            IsProfileInfoActive = true;
            IsPostInfoActive = false;
            CurrentView = new ProfileView();
        }

        private void PostInfo_Click(object sender, RoutedEventArgs e)
        {
            IsGologinInfoActive = false;
            IsProfileInfoActive = false;
            IsPostInfoActive = true;
            CurrentView = new PostView();
        }
        #endregion

    }
}