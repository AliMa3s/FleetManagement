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

namespace FleetManagement.WPF.SelecteerWindows
{
    /// <summary>
    /// Interaction logic for SelecteerAutoModel.xaml
    /// </summary>
    public partial class SelecteerAutoModel : Window
    {
        private readonly AutoModelManager _manager;
        private AutoModel _autoModel;

        public string Placeholder { get; } = "Merk + Automodel";

        public AutoModel AutoModel
        {
            get => _autoModel;
            set
            {
                _autoModel = value;
                AutoModellenLijst.SelectedItem = value;
            }
        }

        public SelecteerAutoModel(AutoModelManager autoModelManager)
        {
            InitializeComponent();
            _manager = autoModelManager;

            AutoModellenLijst.ItemsSource = _manager.FilterOpAutoModelNaam("");
            TextBoxFilterAutonaam.Text = Placeholder;
        }

        //Automodel bewaren telkens een Model wordt geselecteerd
        private void BewaarAutoModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoModel = AutoModellenLijst.SelectedItem as AutoModel;
        }

        private void AutomodelToevoegenDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //_automodel mag niet leeg zijn anders geen terugkeer
            if (AutoModel != null)
            {
                DialogResult = true;
            }
        }

        private void ButtonAnnuleer_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TextBoxFilterOpAutoModel_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxFilterAutonaam.Text == Placeholder)
            {
                TextBoxFilterAutonaam.Text = string.Empty;
                TextBoxFilterAutonaam.Foreground = Brushes.Black;
                AutoModellenLijst.SelectedItem = null;
            }
        }

        private void TextBoxFilterOpAutoModel_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxFilterAutonaam.Text))
            {
                TextBoxFilterAutonaam.Text = Placeholder;
                TextBoxFilterAutonaam.Foreground = Brushes.LightSlateGray;
            }
        }

        private void VoegAutomodelToe_Click(object sender, RoutedEventArgs e)
        {
            //_automodel mag niet leeg zijn anders geen terugkeer
            if (AutoModel != null)
            {
                DialogResult = true;
            }
        }

        private void TextBoxFilterAutonaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(TextBoxFilterAutonaam.Text != Placeholder)
            {
                AutoModellenLijst.ItemsSource = _manager.FilterOpAutoModelNaam(TextBoxFilterAutonaam.Text);
            }
        }
    }
}
