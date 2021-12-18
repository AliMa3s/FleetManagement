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

namespace FleetManagement.WPF.DetailWindows {
    /// <summary>
    /// Interaction logic for TankkaartDetails.xaml
    /// </summary>
    public partial class TankkaartDetails : Window {

        private readonly Managers _managers;
        private TankKaart _tankkaartDetail;

        public TankkaartDetails(Managers managers, TankKaart tankkaart) {

            InitializeComponent();
            _managers = managers;
            _tankkaartDetail = tankkaart;


            //Checken anders wordt dat telkens bijgewerkt
            if(tankkaart.Brandstoffen.Count < 1)
            {
                var brandstoffen = _managers.TankkaartManager.BrandstoffenVoorTankaart(tankkaart).ToList();

                if(brandstoffen.Count > 0)
                {
                    brandstoffen.ForEach(brandstof => {

                        if(!tankkaart.IsBrandstofAanwezig(brandstof))
                            tankkaart.Brandstoffen.Add(brandstof);
                    });
                }
            }           
            
            

            if (_tankkaartDetail.HeeftTankKaartBestuurder)
            {
                StringBuilder stringBuilder = new("Naam: " + _tankkaartDetail.Bestuurder.Achternaam);
                stringBuilder.Append(" " + _tankkaartDetail.Bestuurder.Voornaam);
                stringBuilder.AppendLine(Environment.NewLine + "Rijksregister: " + _tankkaartDetail.Bestuurder.RijksRegisterNummer);
                BestuurderDetail.Text = stringBuilder.ToString();
            }

            if(_tankkaartDetail.Brandstoffen.Count > 0)
            {
                StringBuilder stringBuilder = new("");

                _tankkaartDetail.Brandstoffen.ForEach(brandstof => {
                    stringBuilder.AppendLine(brandstof.BrandstofNaam);
                });

                Brandstoffen.Text = stringBuilder.ToString();
            }

            if(_tankkaartDetail.Actief)
            {
                ActiefTextBlock.Text = "Kaart is actief";
            }
            else
            {
                ActiefTextBlock.Text = _tankkaartDetail.IsGeldigheidsDatumVervallen ? "Datum is verstreken" : "Geblokkeerd";
            }

            //bind de tankaart
            DataContext = _tankkaartDetail;
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

        private void WijzigButton_Click(object sender, RoutedEventArgs e) {

            UpdateTankkaart updateTankkaart = new(_managers, _tankkaartDetail)
            {
                Owner = Window.GetWindow(this),
            };

            bool? updatetet = updateTankkaart.ShowDialog();
            if (updatetet == true)
            {
                
            }
        }
    }
}
