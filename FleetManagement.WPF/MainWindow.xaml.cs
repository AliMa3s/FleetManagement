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

        public MainWindow(UnitOfManager unitOfManager)
        {
            InitializeComponent(); 
            _manager = unitOfManager;

            _manager.Auth.Roles.ToList().ForEach(name =>
            {
                auth.Items.Add(name.Key);
            });
            
            _manager.VoertuigBouwer = new VoertuigBouwer(_manager.VoertuigManager)
            {
                VoertuigKleur = null,
                AantalDeuren = null
            };
        }
         
        private void AuthNameSelectie(object sender, SelectionChangedEventArgs e)
        {
            Toevoegen.IsEnabled = false;
            Zoeken.IsEnabled = false;

            string checkAuth = auth.SelectedItem.ToString();
            if (_manager.Auth.Roles.ContainsKey(checkAuth))
            {
                Role loggedIn = _manager.Auth.Roles[checkAuth];
                if (loggedIn.IsAdmin)
                {
                    Toevoegen.IsEnabled = true;
                    Zoeken.IsEnabled = true; 
                }
                else
                {
                    Zoeken.IsEnabled = true;
                    Toevoegen.IsEnabled = false;
                }

                _manager.LoggedIn = loggedIn;
            }
            else
            {
                _manager.LoggedIn = null;
            }
        }

        private void Zoeken_Click(object sender, RoutedEventArgs e)
        {
            var window = new ZoekWindow(_manager)
            {
                Owner = this
            };

            window.Show();
        }

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            var window = new ToevoegWindow(_manager)
            {
                Owner = this
            };

            window.Show();
        }

        private void Afsluiten_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
