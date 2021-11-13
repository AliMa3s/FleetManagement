using FleetManagement.Bouwers;
using FleetManagement.Manager;
using FleetManagement.Manager.Roles;
using FleetManagement.Model;
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
        private readonly UnitOfManager _manager;

        #region ctor & dependency injection
        public MainWindow(UnitOfManager unitOfManager)
        {
            InitializeComponent(); 
            _manager = unitOfManager;

            _manager.Auth.Roles.ToList().ForEach(name =>
            {
                Auth.Items.Add(name.Key);
            });
        }
        #endregion

        #region toekenning van de rechten
        private void AuthNameSelectie(object sender, SelectionChangedEventArgs e)
        {
            Toevoegen.IsEnabled = false;
            Zoeken.IsEnabled = false;
            WagenparkBeheer.IsEnabled = false;

            string checkAuth = Auth.SelectedItem.ToString();
            if (_manager.Auth.Roles.ContainsKey(checkAuth))
            {
                Role loggedIn = _manager.Auth.Roles[checkAuth];
                if (loggedIn.IsAdmin)
                {
                    Toevoegen.IsEnabled = true;
                    Zoeken.IsEnabled = true;
                    WagenparkBeheer.IsEnabled = true;
                }
                else
                {
                    Zoeken.IsEnabled = true;
                    Toevoegen.IsEnabled = false;
                    WagenparkBeheer.IsEnabled = false;
                }

                _manager.LoggedIn = loggedIn;
            }
            else
            {
                _manager.LoggedIn = null;
            }
        }
        #endregion

        #region nieuwe windowschermen na klikken
        private void Zoeken_Click(object sender, RoutedEventArgs e)
        {
            Window window = new ZoekWindow(_manager)
            {
                Owner = this
            };

            window.Show();
        }

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            Window window = new ToevoegWindow(_manager)
            {
                Owner = this
            };

            window.Show();
        }

        private void WagenparBeheer_Click(object sender, RoutedEventArgs e)
        {
            Window window = new WagenparkBeheer(_manager)
            {
                Owner = this
            };

            window.Show();
        }
        #endregion

        private void Afsluiten_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
