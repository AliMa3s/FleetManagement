using FleetManagement.Manager;
using FleetManagement.WPF.UserControls.Zoeken;
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
using System.Windows.Shapes;

namespace FleetManagement.WPF
{
    /// <summary>
    /// Interaction logic for ZoekWindow.xaml
    /// </summary>
    public partial class ZoekWindow : Window
    {
        private readonly Managers _managers;

           
        public ZoekWindow(Managers managers)
        {
            InitializeComponent();
            Title = "Fleet Management Zoeken & Updaten";
            _managers = managers;

            VoertuigZoekTab.Content = new VoertuigZoeken(_managers);  
        }

        private void BestuurderZoekTab_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!BestuurderZoekTab.IsSelected)
            {
                BestuurderZoekTab.Content = new BestuurderZoeken(_managers);
            }
        }

        private void VoertuigZoekTab_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!VoertuigZoekTab.IsSelected)
            {
                VoertuigZoekTab.Content = new VoertuigZoeken(_managers);
            }
        }

        private void TankkaartZoekTab_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!TankkaartZoekTab.IsSelected)
            {
                TankkaartZoekTab.Content = new TankkaartZoeken(_managers); 
            }
        }

        private void AutomodelZoekTab_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AutomodelZoekTab.IsSelected)
            {
                AutomodelZoekTab.Content = new AutoModelZoeken(_managers);
            }
        }
    }
}
