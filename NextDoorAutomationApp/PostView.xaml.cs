using Business.Constans;
using Business.Dao;
using Business.Models;
using PuppeteerSharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace NextDoorAutomationApp
{
    /// <summary>
    /// Interaction logic for PostView.xaml
    /// </summary>
    public partial class PostView : UserControl
    {
        #region bien
        private int pageIndex = 1;
        private int pageSize = 20;
        private int totalPosts;
        #endregion

        public PostView()
        {
            InitializeComponent();
            LoadData();
            SetDefault();
            DataContext = this;
        }

        #region ham
        private void LoadData()
        {
            // Giả sử bạn có một phương thức lấy dữ liệu từ API hoặc database.
            // Thay thế nó với dữ liệu thực tế.
            var posts = Search();
            PostDataGrid.ItemsSource = posts;
            UpdatePageNumber();
        }

        private void SetDefault()
        {
            // Lấy ngày hôm nay
            DateTime today = DateTime.Today;
            //// Thiết lập ngày bắt đầu là 1 tuần trước
            //StartDatePicker.SelectedDate = today.AddDays(-7);
            //// Thiết lập ngày kết thúc là hôm nay
            //EndDatePicker.SelectedDate = today;
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;

            StatusFilter.SelectedIndex = 1;
            PageSizeComboBox.SelectedIndex = 1;
        }

        private List<PostInfo> Search()
        {
            // Phương thức giả lập lấy dữ liệu các bài viết, thay thế với truy vấn thực tế.
            return PostDao.GetInstance().Search(out totalPosts, pageSize, pageIndex, NameFilter.Text, 
                                        StartDatePicker.SelectedDate, EndDatePicker.SelectedDate, GetStatus());
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
            PageNumberText.Text = $"{pageIndex}/{Math.Ceiling((double)totalPosts / pageSize)}";
            TotalPostsText.Text = $"Total posts: {totalPosts}";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý tìm kiếm theo bộ lọc
            LoadData();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (pageIndex * pageSize < totalPosts)
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
                    box.Background = (ImageBrush)FindResource("Search by neighborhood name");
                else
                    box.Background = null;
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy ID của hàng hiện tại
            var button = sender as Button;
            var info = button?.DataContext as PostInfo; // Đảm bảo rằng DataContext là item của danh sách dữ liệu
            if (info != null)
            {
                //todo - send message
                info.Status = (int)POST_STATUS.SENT;
                PostDao.GetInstance().Replace(info);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy ID của hàng hiện tại
            var button = sender as Button;
            var info = button?.DataContext as PostInfo; // Đảm bảo rằng DataContext là item của danh sách dữ liệu
            if (info != null)
            {
                info.Status = (int)POST_STATUS.CANCEL;
                PostDao.GetInstance().Replace(info);
            }
            LoadData();
        }
        #endregion
    }
}
