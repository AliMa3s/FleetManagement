using FleetManagement.Filters;
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

namespace FleetManagement.WPF
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        private readonly Managers _managers;

        public Filter Filter { get; set; }

        public FilterWindow(Managers managers, Filter filter)
        {
            InitializeComponent();
            _managers = managers;
            Filter = filter;

            if (Filter == null)
            {
                Filter = new(
                    new(),
                    new(),
                    new()
                );
            }

            CheckBox checkboxHybride = new();
            checkboxHybride.FontSize = 15;
            checkboxHybride.Name = "HybrideCheckbox";
            checkboxHybride.VerticalContentAlignment = VerticalAlignment.Center;
            checkboxHybride.Content = "Hybride";
            if(Filter != null)
            {
                checkboxHybride.IsChecked = Filter.Hybride;
                HybrideCheckBox.Children.Add(checkboxHybride);
            }


            _managers.Brandstoffen.ToList().ForEach(brandstof => {

                CheckBox checkBox = new();

                if(Filter != null && Filter.Brandstoffen.Contains(brandstof.BrandstofNaam))
                    checkBox.IsChecked = true;

                checkBox.Content = brandstof.BrandstofNaam;
                checkBox.FontSize = 15;
                checkBox.VerticalContentAlignment = VerticalAlignment.Center;
                BrandstofCheckBoxes.Children.Add(checkBox);
            });

            _managers.Kleuren.ToList().ForEach(kleur => {

                CheckBox checkBox = new();

                if (Filter != null && Filter.Kleuren.Contains(kleur.KleurNaam))
                    checkBox.IsChecked = true;

                checkBox.Content = kleur.KleurNaam;
                checkBox.FontSize = 15;
                checkBox.VerticalContentAlignment = VerticalAlignment.Center;
                KleurCheckBoxes.Children.Add(checkBox);
            });

            
            _managers.AutoTypes.ToList().ForEach(autotype => {

                CheckBox checkBox = new();

                if (Filter != null && Filter.AutoTypes.Contains(autotype.Value))
                    checkBox.IsChecked = true;

                checkBox.Content = autotype.Value;
                checkBox.FontSize = 15;

                checkBox.VerticalContentAlignment = VerticalAlignment.Center;
                AutoTypeCheckBoxes.Children.Add(checkBox);
            });
        }

        private void VoegtoeButton_Click(object sender, RoutedEventArgs e)
        {
            Filter.Brandstoffen.Clear();

            foreach (CheckBox item in BrandstofCheckBoxes.Children)
            {
                if (item.IsChecked == true)
                {
                   Filter.Brandstoffen.Add(item.Content.ToString());
                }             
            }

            Filter.AutoTypes.Clear();

            foreach (CheckBox item in AutoTypeCheckBoxes.Children)
            {
                if (item.IsChecked == true)
                {
                    Filter.AutoTypes.Add(item.Content.ToString());
                }
            }

            Filter.Kleuren.Clear();

            foreach (CheckBox item in KleurCheckBoxes.Children)
            {
                if (item.IsChecked == true)
                {
                    Filter.Kleuren.Add(item.Content.ToString());
                }
            }

            Filter.Hybride = false;
            foreach (CheckBox item in HybrideCheckBox.Children)
            {
                if (item.Content.ToString() == "Hybride" && item.IsChecked == true)
                {
                    Filter.Hybride = true;
                    break;
                }
            }

            if (Filter.Brandstoffen.Count < 1 && Filter.Kleuren.Count < 1 && Filter.AutoTypes.Count < 1 && !Filter.Hybride)
            {
                DialogResult = false;
            }
            else
            {
                DialogResult = true;
            }
        }

        private void AnnuleerForm_Click(object sender, RoutedEventArgs e)
        {
            Filter.Brandstoffen.Clear();
            Filter.AutoTypes.Clear();
            Filter.Kleuren.Clear();
            Filter.Hybride = false;

            DialogResult = false;
        }
    }
}
