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

        public string PlaceholderModelNaam { get; } = "Merk + Automodel";

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
                GetUpdateWindow();
            }
        }

        private void SluitWindow_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void KiesUpdate_Click(object sender, RoutedEventArgs e) {
            if (AutoModel != null)
            {
                infoAutoModelMess.Text = string.Empty;
                GetUpdateWindow();
            }
        }

        private void FilterOpAutoModel_GotFocus(object sender, RoutedEventArgs e) 
        {
            if (FilterOpAutoModel.Text == PlaceholderModelNaam)
            {
                FilterOpAutoModel.Text = string.Empty;
                FilterOpAutoModel.Foreground = Brushes.Black;
                AutoModellenLijst.SelectedItem = null;
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
                infoAutoModelMess.Text = string.Empty;
                _filterOpAutoModel = FilterOpAutoModel.Text;
                FilterAutoModel();
            }
        }

        private void GetUpdateWindow()
        {
            infoAutoModelMess.Text = string.Empty;

            UpdateAutoModel detailWindow = new(_managers, AutoModel)
            {
                Owner = Window.GetWindow(this),
            };

            bool? updatet = detailWindow.ShowDialog();
            if ((bool)updatet)
            {
                FilterAutoModel();
                infoAutoModelMess.Foreground = Brushes.Green;
                infoAutoModelMess.Text = "Succesvol aangepast";
            }
        }

        private void ZoekOpAutoTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            infoAutoModelMess.Text = string.Empty;
            FilterAutoModel();
        }

        private void FilterAutoModel()
        {
            infoAutoModelMess.Text = string.Empty;

            if (ZoekOpAutoTypes.SelectedItem.ToString() != "Alles weergeven")
            {
                AutoModellenLijst.ItemsSource = _managers.AutoModelManager.ZoekOpAutoType(new(ZoekOpAutoTypes.SelectedItem.ToString()), _filterOpAutoModel);
            }
            else
            {
                AutoModellenLijst.ItemsSource = _managers.AutoModelManager.FilterOpAutoModelNaam(_filterOpAutoModel);
            }
        }
    }
}
