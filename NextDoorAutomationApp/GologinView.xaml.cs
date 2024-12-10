using Business.Business;
using Business.Constans;
using Business.Dao;
using Business.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NextDoorAutomationApp
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
    }
}
