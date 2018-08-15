using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Scanner_Virtual_Keyboard.Configuration
{
    /// <summary>
    /// Logique d'interaction pour ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        private Configuration config;

        public ConfigurationWindow()
        {
            InitializeComponent();
            config = Configuration.Load();
            PortConfigPanel.DataContext = config.Port;
            KeyboardConfigPanel.DataContext = config.Keyboard;
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            config.Save();
            DialogResult = true;
            this.Close();
        }

    }
}
