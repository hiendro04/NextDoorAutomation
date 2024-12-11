using Business.Dao;
using Business.Models;
using NextDoorAutomationApp.Views.Profile;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NextDoorAutomationApp.Views.Gologin
{
    /// <summary>
    /// Interaction logic for GologinView.xaml
    /// </summary>
    public partial class GologinView : UserControl
    {
        #region bien
        private int pageIndex = 1;
        private int pageSize = 20;
        private int total;
        #endregion

        public GologinView()
        {
            InitializeComponent();
            LoadData();
            SetDefault();
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
            PageSizeComboBox.SelectedIndex = 1;
        }

        private List<GologinInfo> Search()
        {
            // Phương thức giả lập lấy dữ liệu các bài viết, thay thế với truy vấn thực tế.
            return GologinDao.GetInstance().Search(out total, pageSize, pageIndex, NameFilter.Text);
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
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new GologinAddView();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var info = button?.DataContext as GologinInfo;

            // Thay đổi màn hình
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new GologinAddView(info);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var info = button?.DataContext as GologinInfo;

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
                GologinDao.GetInstance().Delete(info._id);
                LoadData();
            }
        }

        private void ListProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var info = button?.DataContext as GologinInfo;

            if (info == null) return;

            // Thay đổi màn hình
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new ProfileView(info);
        }
    }
}
