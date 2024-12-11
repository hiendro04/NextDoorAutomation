using Business.Dao;
using Business.Models;
using MongoDB.Bson;
using System.Windows;
using System.Windows.Controls;

namespace NextDoorAutomationApp.Views.Gologin
{
    /// <summary>
    /// Interaction logic for GologinAddView.xaml
    /// </summary>
    public partial class GologinAddView : UserControl
    {
        private ObjectId Id = ObjectId.Empty;
        public GologinAddView()
        {
            InitializeComponent();
            TitleInput.Text = "Add New Gologin";
        }

        public GologinAddView(GologinInfo info)
        {
            InitializeComponent();
            TitleInput.Text = "Edit Gologin";
            Id = info._id;
            NameInput.Text = info.Name;
            AccessTokensInput.Text = info.AccessTokens;
            DescriptionInput.Text = info.Description;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            // validate
            if (string.IsNullOrWhiteSpace(NameInput.Text))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(AccessTokensInput.Text))
            {
                MessageBox.Show("Access tokens are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            //save
            if(Id == ObjectId.Empty)
            {
                var info = new GologinInfo();
                info.Name = NameInput.Text;
                info.AccessTokens = AccessTokensInput.Text;
                info.Description = DescriptionInput.Text;
                info.CreatedDate = DateTime.Now;

                GologinDao.GetInstance().Insert(info);
            }
            else
            {
                var oldInfo = GologinDao.GetInstance().GetById(Id);
                oldInfo.Name = NameInput.Text;
                oldInfo.AccessTokens = AccessTokensInput.Text;
                oldInfo.Description = DescriptionInput.Text;

                GologinDao.GetInstance().Replace(oldInfo);
            }

            
            // Chuyển về màn hình ProfileView
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new GologinView();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển về màn hình ProfileView
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new GologinView();
        }
    }
}
