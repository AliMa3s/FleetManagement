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

namespace FleetManagement.WPF.UserControls.Zoeken
{
    /// <summary>
    /// Interaction logic for TankkaartZoeken.xaml
    /// </summary>
    public partial class TankkaartZoeken : UserControl
    {
        private readonly Managers _managers;

        private TankKaart _tankkaart;

        public TankKaart TankkaartWeergave
        {
            get => _tankkaart;
            set
            {
                _tankkaart = value;
                ZoekweergaveTankkaart.SelectedItem = value;
            }
        }
        

        public TankkaartZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;

            ZoekweergaveTankkaart.ItemsSource = _managers.TankkaartManager.GeefAlleTankkaart();
        }

        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TankkaartWeergave = ZoekweergaveTankkaart.SelectedItem as TankKaart;
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_tankkaart != null)
            {
                TankkaartDetails detailWindow = new TankkaartDetails(_managers, _tankkaart)
                {
                    Owner = Window.GetWindow(this),
                };

                //Uitgezet anders geen update status mogelijk
                //detailWindow.Show();

                bool? verwijderd = detailWindow.ShowDialog();
                if(verwijderd == true)
                {
                    ZoekweergaveTankkaart.ItemsSource = _managers.TankkaartManager.GeefAlleTankkaart();
                }
            }
        }

        private void SluitVoertuigForm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Verwijderen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBoxFilterTextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void KiesDetail_Click(object sender, RoutedEventArgs e)
        {
            if (_tankkaart != null)
            {
                TankkaartDetails detailWindow = new TankkaartDetails(_managers, _tankkaart)
                {
                    Owner = Window.GetWindow(this),
                };

                detailWindow.Show();
            }
        }

        private void SluitVoertuigForm_Click_1(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
