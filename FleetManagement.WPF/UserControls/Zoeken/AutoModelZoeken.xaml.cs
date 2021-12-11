using FleetManagement.Manager;
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
    /// Interaction logic for AutoModelZoeken.xaml
    /// </summary>
    public partial class AutoModelZoeken : UserControl
    {
        private readonly Managers _managers;


        public AutoModelZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;


        }

        private void KiesDetail_Click(object sender, RoutedEventArgs e) {

        }

        private void SluitAutoModelForm_Click(object sender, RoutedEventArgs e) {

        }

        private void ZoekmodelNaamAutoType_Click(object sender, RoutedEventArgs e) {

        }

        private void FilterOpModelNaamEnAutoType_Changed(object sender, TextChangedEventArgs e) {

        }

        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e) {

        }

        private void Merknaam_GotFocus(object sender, RoutedEventArgs e) {

        }

        private void Merknaam_LostFocus(object sender, RoutedEventArgs e) {

        }

        private void FilterOpMerkEnAutomdel_Changed(object sender, TextChangedEventArgs e) {

        }

        private void ModelNaamEnAutoType_GotFocus(object sender, RoutedEventArgs e) {

        }

        private void ModelNaamEnAutoType_LostFocus(object sender, RoutedEventArgs e) {

        }
    }
}
