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

namespace FleetManagement.WPF.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateTankkaart.xaml
    /// </summary>
    public partial class UpdateTankkaart : Window
    {
        private readonly Managers _managers;
        private List<string> _keuzeBrandstoffen;

        public string DisplayFirst { get; set; } = "Selecteer";

        private TankKaart _tankaart;

        public TankKaart TankkaartDetail
        {
            get => _tankaart;
            set
            {
                _tankaart = value;
            }
        }

        public UpdateTankkaart(Managers managers, TankKaart tankkaart)
        {
            InitializeComponent();
            _managers = managers;
            _tankaart = tankkaart;

            FormTankkaart.Content = "Tankkaart Updaten";


            BrandstofNamenComboBox.Items.Add(DisplayFirst);
            _managers.Brandstoffen.ToList().ForEach(brandstof => {

                
                if(tankkaart.Brandstoffen.Count > 0)
                {
                    if (!tankkaart.IsBrandstofAanwezig(brandstof))
                    {
                        BrandstofNamenComboBox.Items.Add(brandstof.BrandstofNaam);
                    }
                }
            });

            PincodeUpdate.Text = _tankaart.Pincode;
        }

        private void TankKaartUpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetVeldenButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SluitTankKaartWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetGekozenBrandstofButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BrandstofToevoegenButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TankkaartAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}







