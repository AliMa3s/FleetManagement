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

            //Controleer bestuurder om extra info op te vragen aan manager
            if (!tankkaart.HeeftTankKaartBestuurder || tankkaart.Brandstoffen.Count < 1)
            {
                //tankkaart = managers.TankkaartManager.TankkaartIncludes(tankkaart);  //interface 
            }

            if(tankkaart.HeeftTankKaartBestuurder)
            {
                StringBuilder stringBuilder = new("Naam: " + tankkaart.Bestuurder.Achternaam + "" + tankkaart.Bestuurder.Voornaam);
                stringBuilder.AppendLine("Rijksregisternr.: " + tankkaart.Bestuurder.RijksRegisterNummer);
                BestuurderDetail.Text = stringBuilder.ToString();
            }

            if(tankkaart.Brandstoffen.Count > 0)
            {
                StringBuilder stringBuilder = new("Geldig voor deze brandstoffen:");

                tankkaart.Brandstoffen.ForEach(brandstof => {
                    stringBuilder.AppendLine(brandstof.BrandstofNaam);
                });

                Brandstoffen.Text = stringBuilder.ToString();
            }

            if(tankkaart.Pincode != null)
            {
                Pincode.Text = tankkaart.Pincode;
            }

            if(tankkaart.Actief)
            {
                ActiefTextBlock.Text = "Kaart is actief";
            }
            else
            {
                ActiefTextBlock.Text = tankkaart.IsGeldigheidsDatumVervallen ? "Datum is verstreken" : "Geblokkeerd";
            }
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void WijzigButton_Click(object sender, RoutedEventArgs e) {

            UpdateTankkaart updateTankkaart = new(_managers, _tankkaartDetail)
            {
                Owner = Window.GetWindow(this),
                TankkaartDetail = _tankkaartDetail
            };

            bool? updatetet = updateTankkaart.ShowDialog();
            if (updatetet == true)
            {
                _tankkaartDetail = updateTankkaart.TankkaartDetail;

                //Changer aanspreken indien nodig
            }
        }
    }
}
