using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Scanner_Virtual_Keyboard
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool ActivateClose = false;
        private SerialMonitor monitor = null;
        public ObservableCollection<ScanItem> HistoryScan { get; set; }
        public Configuration.Configuration Config;
        public string SerialStatus { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            App.Current.SessionEnding += Current_SessionEnding;
            SerialStatus = "Waiting device ...";

            Config = Configuration.Configuration.Load();
            monitor = new SerialMonitor(Config.Port);
            monitor.OnReceived += monitor_OnReceived;
            monitor.OnStatusChanged += Monitor_OnStatusChanged;

            HistoryScan = new ObservableCollection<ScanItem>();
            MainGrid.DataContext = this;
        }



        private void MenuItem_Click_Options(object sender, RoutedEventArgs e)
        {
            Configuration.ConfigurationWindow window = new Configuration.ConfigurationWindow();
            monitor.Stop();
            if (window.ShowDialog() == true)
            {
                //Reload Configuration
                monitor = null;
                Config = Configuration.Configuration.Load();
                monitor = new SerialMonitor(Config.Port);
                monitor.OnReceived += monitor_OnReceived;
                monitor.OnStatusChanged += Monitor_OnStatusChanged;
            }
            monitor.Start();
        }

        private void MenuItem_Click_Quit(object sender, RoutedEventArgs e)
        {
            ActivateClose = true;
            this.Close();
        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            monitor.Stop();
            about.ShowDialog();
            monitor.Start();
        }
        
        private void MenuItem_Click_VirtualKeyboard(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            if (mi.IsChecked)
            {
                VirtualKeyboardPanel.Visibility = System.Windows.Visibility.Visible;
                VirtualKeyboardPanel.Width = 200;
            }
            else
            {
                VirtualKeyboardPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Button_Click_NumPad(object sender, RoutedEventArgs e)
        {
            Button bu = (Button)sender;
            string b_key = (string)bu.Content;
            switch (b_key)
            { 
                case "0":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD0);
                    break;
                case "1":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD1);
                    break;
                case "2":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD2);
                    break;
                case "3":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD3);
                    break;
                case "4":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD4);
                    break;
                case "5":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD5);
                    break;
                case "6":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD6);
                    break;
                case "7":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD7);
                    break;
                case "8":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD8);
                    break;
                case "9":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD9);
                    break;
                case "Entrée":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.RETURN);
                    break;
                case "Verr":
                    WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMLOCK);
                    break;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ActivateClose)
            {
                e.Cancel = true;
                WindowState = System.Windows.WindowState.Minimized;
                return;
            }
            monitor.Stop();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            monitor.Start();
        }

        private void Current_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            this.ActivateClose = true;
            this.Close();
        }

        #region Montior Event

        private void monitor_OnReceived(string obj)
        {
            Dispatcher.Invoke(new Action<string>(SerialDataReceived), obj);
        }

        private void SerialDataReceived(string obj)
        {
            ScanItem item = new ScanItem(obj, Config.Keyboard);
            item.Keystroke();
            HistoryScan.Add(item);
        }
        private void Monitor_OnStatusChanged(string obj)
        {
            Dispatcher.Invoke(new Action<string>(UpdateSerialStatus), obj);
        }

        private void UpdateSerialStatus(string message)
        {
            this.SerialStatus = message;
            PropertyChanged(this, new PropertyChangedEventArgs("SerialStatus"));
        }



        #endregion

        #region Disable Window Focus (Like Virtual Keyboard) and Maximize Button

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private void Window_Activated(object sender, EventArgs e)
        {
            //Set the window style to noactivate.
            WindowInteropHelper helper = new WindowInteropHelper(this);
            //Disable window focus
            SetWindowLong(helper.Handle, GWL_EXSTYLE, GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
            //Disable maximize button
            SetWindowLong(helper.Handle, GWL_STYLE, GetWindowLong(helper.Handle, GWL_STYLE) & ~WS_MAXIMIZEBOX);
        }

        #endregion


    }
}
