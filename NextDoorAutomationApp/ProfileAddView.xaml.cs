using Business.Constans;
using Business.Dao;
using Business.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NextDoorAutomationApp
{
    /// <summary>
    /// Interaction logic for AddProfile.xaml
    /// </summary>
    public partial class ProfileAddView : UserControl
    {
        private GologinInfo _gologinInfo;
        private ObjectId Id = ObjectId.Empty;
        public ProfileAddView(GologinInfo gologinInfo)
        {
            InitializeComponent();
            TypeInput.SelectedIndex = (int)PROFILE_TYPE.SPAM;
            _gologinInfo = gologinInfo;
        }

        public ProfileAddView(GologinInfo gologinInfo, ProfileInfo profileInfo)
        {
            InitializeComponent();
            Id = profileInfo._id;
            NameInput.Text = profileInfo.Name;
            IdInput.Text = profileInfo.ProfilePublicId;
            TypeInput.SelectedIndex = profileInfo.Type;
            DescriptionInput.Text = profileInfo.Description;
            _gologinInfo = gologinInfo;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // validate
            if (string.IsNullOrWhiteSpace(NameInput.Text))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(IdInput.Text))
            {
                MessageBox.Show("ID are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //save
            if (Id == ObjectId.Empty)
            {
                var info = new ProfileInfo();
                info.GologinId = _gologinInfo._id;
                info.Name = NameInput.Text;
                info.ProfilePublicId = IdInput.Text;
                info.Type = TypeInput.SelectedIndex;
                info.Description = DescriptionInput.Text;
                info.CreatedDate = DateTime.Now;

                ProfileDao.GetInstance().Insert(info);
            }
            else
            {
                var oldInfo = ProfileDao.GetInstance().GetById(Id);
                oldInfo.Name = NameInput.Text;
                oldInfo.ProfilePublicId = IdInput.Text;
                oldInfo.Type = TypeInput.SelectedIndex;
                oldInfo.Description = DescriptionInput.Text;

                ProfileDao.GetInstance().Replace(oldInfo);
            }

            // Chuyển về màn hình ProfileView
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new ProfileView(_gologinInfo);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển về màn hình ProfileView
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new ProfileView(_gologinInfo);
        }
    }
}
