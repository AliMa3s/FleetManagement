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

        public bool? Updatetet { get; set; } = false;
        public TankKaart Tankkaart => _tankkaartDetail;

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
                        {
                            tankkaart.Brandstoffen.Add(brandstof);
                        }
                    });
                }
            }

            SetDefault();

            //bind de tankaart
            DataContext = _tankkaartDetail;
        }

        private void SetDefault()
        {
            if (_tankkaartDetail.HeeftTankKaartBestuurder)
            {
                StringBuilder stringBuilder = new("Naam: " + _tankkaartDetail.Bestuurder.Achternaam);
                stringBuilder.Append(" " + _tankkaartDetail.Bestuurder.Voornaam);
                stringBuilder.AppendLine(Environment.NewLine + "Rijksregister: " + _tankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(0, 2) + "."
                    + _tankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(2, 2) + "."
                    + _tankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(4, 2) + "-"
                    + _tankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(6, 3) + "."
                    + _tankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(9, 2));
                BestuurderDetail.Text = stringBuilder.ToString();
            }
            else
            {
               BestuurderDetail.Text = "Geen bestuurder";
            }

            if (_tankkaartDetail.Brandstoffen.Count > 0)
            {
                StringBuilder stringBuilder = new("");

                _tankkaartDetail.Brandstoffen.ForEach(brandstof =>
                {
                    stringBuilder.AppendLine(brandstof.BrandstofNaam);
                });

                Brandstoffen.Text = stringBuilder.ToString();
            }
            else 
            {
                Brandstoffen.Text = "Geen brandstoffen";
            }

            if (_tankkaartDetail.Actief)
            {
                ActiefTextBlock.Text = "Kaart is actief";
            }
            else
            {
                ActiefTextBlock.Text = _tankkaartDetail.IsGeldigheidsDatumVervallen ? "Datum is verstreken" : "Geblokkeerd";
            }
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

        private void WijzigButton_Click(object sender, RoutedEventArgs e) {

            infoTankkaartMess.Text = string.Empty;

            UpdateTankkaart updateTankkaart = new(_managers, _tankkaartDetail)
            {
                Owner = Window.GetWindow(this),
            };

            bool? update = updateTankkaart.ShowDialog();
            if (update == true)
            {
                Updatetet = true;
                _tankkaartDetail = updateTankkaart.TankkaartDetail;

                DataContext = null;
                DataContext = _tankkaartDetail;
                SetDefault();

                infoTankkaartMess.Foreground = Brushes.Green;
                infoTankkaartMess.Text = "Bestuurder succesvol geüpdatet";
            }
        }

        private void VerwijderButton_Click(object sender, RoutedEventArgs e)
        {
            infoTankkaartMess.Text = string.Empty;

            BevestigingWindow bevestigingWindow = new($"Zeker dat je de tankkaart wilt blokkeren?")
            {
                Owner = Window.GetWindow(this),
            };

            bool? blokkeren = bevestigingWindow.ShowDialog();
            if (blokkeren == true)
            {
                infoTankkaartMess.Text = string.Empty;

                try
                {
                    _tankkaartDetail.BlokkeerTankKaart();
                    _managers.TankkaartManager.UpdateTankKaart(_tankkaartDetail);
                    Updatetet = true;

                    DataContext = null;
                    DataContext = _tankkaartDetail;
                    SetDefault(); 
                }
                catch (Exception ex)
                {
                    infoTankkaartMess.Foreground = Brushes.Red;
                    infoTankkaartMess.Text = ex.Message;
                }
            }
        }
    }
}
