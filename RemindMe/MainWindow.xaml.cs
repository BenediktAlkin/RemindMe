using RemindMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RemindMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon NotifyIcon { get; set; }
        private MainWindowViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;

            // set icon of window
            var iconUri = "file:///" + System.IO.Path.GetFullPath("logo_black.ico");
            Icon = BitmapFrame.Create(new Uri(iconUri));

            ShowInTaskbar = false;
            var args = Environment.GetCommandLineArgs();
            // if ui should not be launched it is indicated with the argument "HideUI"
            if (args.Length == 2)
                Visibility = Visibility.Hidden;
                

            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Launch RemindMe", null, Launch_RemindMe);
            contextMenuStrip.Items.Add("Quit", null, Quit);
            NotifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon(System.IO.Path.Combine(Environment.CurrentDirectory, "logo_white.ico")),
                Visible = true,
                ContextMenuStrip = contextMenuStrip,
            };
            NotifyIcon.Click += new EventHandler(NotifyIcon_Click);

            NotificationManager.Instance.StartTimer();
        }

        private void ShowWindow()
        {
            Visibility = Visibility.Visible;
            ViewModel.StartUpdateTask();
        }
        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            var args = e as System.Windows.Forms.MouseEventArgs;
            if (args.Button == MouseButtons.Left)
                ShowWindow();
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
                Visibility = Visibility.Hidden;

                ViewModel.StopUpdateTask();
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NotifyIcon.Visible = false;
            NotificationManager.Instance.ClearNotifications();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                ViewModel.StartUpdateTask();
        }

        #region trac icon menu actions
        private void Launch_RemindMe(object sender, EventArgs e) => ShowWindow();
        private void Quit(object sender, EventArgs e) => Close();
        #endregion

    }
}
