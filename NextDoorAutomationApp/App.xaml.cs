using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace NextDoorAutomationApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Đăng ký sự kiện xử lý lỗi toàn cục
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Xử lý lỗi tại đây
            MessageBox.Show(
                $"An unexpected error occurred: {e.Exception.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            // Ngăn ứng dụng bị thoát
            e.Handled = true;
        }
    }
}
