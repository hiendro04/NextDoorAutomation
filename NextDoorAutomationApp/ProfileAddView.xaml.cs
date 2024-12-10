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
        public ProfileAddView()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //todo

            // Chuyển về màn hình ProfileView
            (Application.Current.MainWindow.DataContext as MainWindow).CurrentView = new ProfileView();
        }
    }
}
