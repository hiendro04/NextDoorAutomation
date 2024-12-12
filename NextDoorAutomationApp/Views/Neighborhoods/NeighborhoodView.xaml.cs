using Business.Constans;
using Business.Dao;
using Business.Models;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NextDoorAutomationApp.Views.Neighborhoods
{
    /// <summary>
    /// Interaction logic for NeighborhoodView.xaml
    /// </summary>
    public partial class NeighborhoodView : UserControl
    {
        #region bien
        private int pageIndex = 1;
        private int pageSize = 20;
        private int total;
        #endregion
        public NeighborhoodView()
        {
            InitializeComponent();
            TitleInput.Text = "List Neighborhoods";
            LoadData();
            SetDefault();
            LoadProfiles();
            DataContext = this;
        }
        private void LoadData()
        {
            var data = Search();
            DataList.ItemsSource = data;
            UpdatePageNumber();
        }

        private void LoadProfiles()
        {
            var profiles = ProfileDao.GetInstance().GetAll().Where(p => p.Type == (int)PROFILE_TYPE.TPP).ToList();
            ProfileComboBox.ItemsSource = profiles;
        }


        private List<NeighborhoodInfo> Search()
        {
            var citys = CityDao.GetInstance().GetAllActive().Where(c => c.Name.ToLower().Contains(CityNameFilter.Text.Trim().ToLower())).ToList();
            var cityIds = citys.Select(c => c._id).ToList();
            // Phương thức giả lập lấy dữ liệu các bài viết, thay thế với truy vấn thực tế.
            var data = NeighborhoodDao.GetInstance().Search(out total, pageSize, pageIndex, NameFilter.Text, cityIds);
            var stateInfo = StateDao.GetInstance().GetAll().FirstOrDefault();
            foreach (var item in data)
            {
                var cityInfo = CityDao.GetInstance().GetById(item.CityId);
                item.StateName = stateInfo.Name;
                item.CityName = cityInfo.Name;
            }
            return data;
        }

        private void SetDefault()
        {
            PageSizeComboBox.SelectedIndex = 1;
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

        private void ProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProfileComboBox.SelectedItem is ProfileInfo selectedProfile)
            {
                MessageBox.Show($"Selected Profile: {selectedProfile.Name}, Id: {selectedProfile.IdStr}");
            }
        }

        private void FollowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MultiFollowButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy danh sách các post đã được chọn
            var selectedList = DataList.ItemsSource.OfType<NeighborhoodInfo>()
                                                      .Where(n => n.IsSelected)
                                                      .ToList();

            // Xử lý danh sách các post đã chọn (ví dụ: thực hiện hành động "Follow" cho các post đã chọn)
            foreach (var n in selectedList)
            {
                // Ví dụ xử lý follow
                Console.WriteLine($"Following post: {n.Name}");
            }

            // Thông báo kết quả
            MessageBox.Show($"{selectedList.Count} posts selected for Multi Follow.");
        }
    }
}
