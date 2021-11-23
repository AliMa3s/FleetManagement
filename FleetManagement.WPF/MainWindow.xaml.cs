using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.DetailWindows;
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

namespace FleetManagement.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Managers _managers;

        #region ctor & dependency injection
        public MainWindow(Managers managers)
        {
            InitializeComponent(); 
            _managers = managers;
        }
        #endregion

        #region nieuwe windowschermen na klikken
        private void Zoeken_Click(object sender, RoutedEventArgs e)
        {
            Window window = new ZoekWindow(_managers)
            {
                Owner = GetWindow(this)
            };

            window.Show();
        }

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            Window window = new ToevoegWindow(_managers)
            {
                Owner = GetWindow(this)
            };

            window.Show();
        }

        private void WagenparBeheer_Click(object sender, RoutedEventArgs e)
        {
            Window window = new BestuurderDetails(_managers, null)
            {
                Owner = GetWindow(this)
            };

            window.Show();
        }
        #endregion

        private void Afsluiten_Click(object sender, RoutedEventArgs e)
        {
            GetWindow(this).Close();
        }
    }
}
