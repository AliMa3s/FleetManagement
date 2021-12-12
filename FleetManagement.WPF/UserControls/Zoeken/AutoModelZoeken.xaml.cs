using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.UpdateWindows;
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
        private readonly Managers _managers;

        private string _filterOpAutoModel = "";

        public string PlaceholderModelNaam { get; } = "Merk + Model";

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
            _managers = managers;

            FilterOpAutoModel.Text = PlaceholderModelNaam;
            AutoModellenLijst.ItemsSource = _managers.AutoModelManager.FilterOpAutoModelNaam("");

            ZoekOpAutoTypes.Items.Add("Alles weergeven");
            _managers.AutoTypes.ToList().ForEach(autotype => {      
                ZoekOpAutoTypes.Items.Add(autotype.Value);
            });
        }

        private void AutoModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoModel = AutoModellenLijst.SelectedItem as AutoModel;
        }

        private void AutoModelMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AutoModel != null)
            {
                GetDetailWindow();
            }
        }

        private void SluitWindow_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void KiesUpdate_Click(object sender, RoutedEventArgs e) {
            if (AutoModel != null)
            {
                GetDetailWindow();
            }
        }

        private void FilterOpAutoModel_GotFocus(object sender, RoutedEventArgs e) 
        {
            if (FilterOpAutoModel.Text == PlaceholderModelNaam)
            {
                FilterOpAutoModel.Text = string.Empty;
                FilterOpAutoModel.Foreground = Brushes.Black;
            }
        }

        private void FilterOpAutoModel_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterOpAutoModel.Text))
            {
                FilterOpAutoModel.Text = PlaceholderModelNaam;
                FilterOpAutoModel.Foreground = Brushes.LightSlateGray;
            }
        }

        private void FilterOpAutoModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterOpAutoModel.Text != PlaceholderModelNaam)
            {
                _filterOpAutoModel = FilterOpAutoModel.Text;

                AutoModellenLijst.ItemsSource = _managers.AutoModelManager.FilterOpAutoModelNaam(_filterOpAutoModel);
            }
        }

        private void GetDetailWindow()
        {
            UpdateAutoModel detailWindow = new(_managers, AutoModel)
            {
                Owner = Window.GetWindow(this),
            };

            bool? updatet = detailWindow.ShowDialog();
            if ((bool)updatet)
            {
                AutoModellenLijst.ItemsSource = _managers.AutoModelManager.FilterOpAutoModelNaam(_filterOpAutoModel);
            }
        }

        private void ZoekOpAutoTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ZoekOpAutoTypes.SelectedItem.ToString() != "Alles weergeven")
            {
                AutoModellenLijst.ItemsSource =_managers.AutoModelManager.ZoekOpAutoType(new(ZoekOpAutoTypes.SelectedItem.ToString()));
            }
            else
            {
                AutoModellenLijst.ItemsSource = _managers.AutoModelManager.FilterOpAutoModelNaam(_filterOpAutoModel);
            }
        }
    }
}
