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

        public FilterWindow(Managers managers)
        {
            InitializeComponent();
            _managers = managers;

            var lijst = _managers.Brandstoffen.ToList();
            lijst.Add(new BrandstofType("Hybride"));
            lijst.Sort();

            lijst.ForEach(brandstof => {

                CheckBox checkBox = new();
                checkBox.Content = brandstof.BrandstofNaam;
                FirstDynamicCheckBoxes.Children.Add(checkBox);
            });
        }
    }
}
