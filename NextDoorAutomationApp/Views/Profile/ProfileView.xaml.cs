using Business.Dao;
using Business.Helpers;
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

namespace NextDoorAutomationApp.Views.Profile
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        private GologinInfo _gologinInfo;
        #region bien
        private int pageIndex = 1;
        private int pageSize = 20;
        private int total;
        #endregion

        public ProfileView()
        {
            InitializeComponent();
            LoadData();
            SetDefault();
            TitleInput.Text = "List Profile";
            DataContext = this;
        }

        public ProfileView(GologinInfo gologinInfo)
        {
            InitializeComponent();
            this._gologinInfo = gologinInfo;
            TitleInput.Text = $"List Profile of {gologinInfo.Name}";
            SetDefault();
            LoadData();
            DataContext = this;
        }

        #region ham
        private void LoadData()
        {
            var data = Search();
            DataList.ItemsSource = data;
            UpdatePageNumber();
        }

        private void SetDefault()
        {
            StatusFilter.SelectedIndex = 0;
            PageSizeComboBox.SelectedIndex = 1;
        }

        private List<ProfileInfo> Search()
        {
            // Phương thức giả lập lấy dữ liệu các bài viết, thay thế với truy vấn thực tế.
            return ProfileDao.GetInstance().Search(out total, pageSize, pageIndex, NameFilter.Text, _gologinInfo.IdStr, GetStatus());
        }

        private int GetStatus()
        {
            var selectedItem = StatusFilter.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                return int.Parse(selectedItem.Tag.ToString());
            }
            return -1;
        }

        private void UpdatePageNumber()
        {
            PageNumberText.Text = $"{pageIndex}/{Math.Ceiling((double)total / pageSize)}";
            TotalText.Text = $"Total: {total}";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý tìm kiếm theo bộ lọc
            LoadData();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (pageIndex * pageSize < total)
            {
                pageIndex++;
                LoadData();
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (pageIndex > 1)
            {
                pageIndex--;
                LoadData();
            }
        }

        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)PageSizeComboBox.SelectedItem;
            pageSize = int.Parse(selectedItem.Content.ToString());
            LoadData();
        }

        private void NameFilterChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (string.IsNullOrEmpty(box.Text))
                    box.Background = (ImageBrush)FindResource("Search by name");
                else
                    box.Background = null;
            }
        }
        #endregion

        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            // Thay đổi màn hình
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new ProfileAddView(_gologinInfo);
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var info = button?.DataContext as ProfileInfo;

            // Thay đổi màn hình
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new ProfileAddView(_gologinInfo, info);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var info = button?.DataContext as ProfileInfo;

            if (info == null) return;

            // Hiển thị thông báo xác nhận
            var result = MessageBox.Show(
                $"Are you sure you want to delete the item: {info.Name}?",
                "Delete Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            // Nếu người dùng chọn "Yes"
            if (result == MessageBoxResult.Yes)
            {
                ProfileDao.GetInstance().Delete(info._id);
                LoadData();
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var info = button?.DataContext as ProfileInfo;
                if (info == null) return;
                var gologinInfo = GologinDao.GetInstance().GetById(info.GologinId);
                var gologinHelper = new GoLoginApiHelper(gologinInfo.AccessTokens);
                var wsUrl = await gologinHelper.StartProfileAsync(info.ProfilePublicId);

                var aa = new WsUrlInfo()
                {
                    Name = info.Name,
                    ProfileId = info.ProfilePublicId,
                    WsUrl = wsUrl,
                    CreatedDate = DateTime.Now,
                    DelayTime = 10
                };
                WsUrlDao.GetInstance().Insert(aa);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
