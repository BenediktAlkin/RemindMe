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

        public MainWindow()
        {
            InitializeComponent();
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
                Icon = new System.Drawing.Icon(System.IO.Path.Combine(Environment.CurrentDirectory, "logo.ico")),
                Visible = true,
                ContextMenuStrip = contextMenuStrip,
            };
            NotifyIcon.Click += new EventHandler(ShowWindow);

            NotificationManager.Instance.StartTimer();
        }

        private void ShowWindow(object sender, EventArgs e)
        {
            var args = e as System.Windows.Forms.MouseEventArgs;
            if (args.Button == MouseButtons.Left)
                Visibility = Visibility.Visible;
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
                Visibility = Visibility.Hidden;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NotifyIcon.Visible = false;
            NotificationManager.Instance.ClearNotifications();
        }

        #region trac icon menu actions
        private void Launch_RemindMe(object sender, EventArgs e) => Visibility = Visibility.Visible;
        private void Quit(object sender, EventArgs e) => Close();
        #endregion
    }
}
