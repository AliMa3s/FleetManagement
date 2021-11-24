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

            //Controleer tankkaart om extra info op te vragen aan manager
            if (!_tankkaartDetail.HeeftTankKaartBestuurder || _tankkaartDetail.Brandstoffen.Count < 1)
            {
                //_tankkaartDetail = managers.TankkaartManager.TankkaartIncludes(_tankkaartDetail);  //interface 
            }

            if (_tankkaartDetail.HeeftTankKaartBestuurder)
            {
                StringBuilder stringBuilder = new("Naam: " + _tankkaartDetail.Bestuurder.Achternaam + "" + _tankkaartDetail.Bestuurder.Voornaam);
                stringBuilder.AppendLine("Rijksregisternr.: " + _tankkaartDetail.Bestuurder.RijksRegisterNummer);
                BestuurderDetail.Text = stringBuilder.ToString();
            }

            if(_tankkaartDetail.Brandstoffen.Count > 0)
            {
                StringBuilder stringBuilder = new("Geldig voor deze brandstoffen:");

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
