using FleetManagement.Manager;
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
using System.Windows.Shapes;

namespace FleetManagement.WPF.UserControls.Zoeken
{
    /// <summary>
    /// Interaction logic for AutoModelZoeken.xaml
    /// </summary>
    public partial class AutoModelZoeken : UserControl
    {
        private readonly Managers _manager;


        private AutoModel _autoModel;
        public AutoModel AutoModel {
            get => _autoModel;
            set {
                _autoModel = value;
                AutoModellenLijst.SelectedItem = value;
            }
        }

        public AutoModelZoeken(Managers managers)
        {
            InitializeComponent();
            _manager = managers;

            AutoModellenLijst.ItemsSource = _manager.AutoModelManager.FilterOpAutoModelNaam("");


        }



        private void TextBoxFilterAutonaam_TextChanged(object sender, TextChangedEventArgs e) {
            //AutoModellenLijst.ItemsSource = _manager.AutoModelManager.FilterOpAutoModelNaam(TextBoxFilterAutonaam.Text);
        }

        private void BewaarAutoModel_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void AutomodelToevoegenDoubleClick(object sender, MouseButtonEventArgs e) {

        }

        private void SluitWindow_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void KiesDetail_Click(object sender, RoutedEventArgs e) {

        }

        private void FilterOpNaam_GotFocus(object sender, RoutedEventArgs e) {

        }

        private void FilterOpNaam_LostFocus(object sender, RoutedEventArgs e) {

        }

        private void FilterOpNaam_TextChanged(object sender, TextChangedEventArgs e) {

        }
    }
}
