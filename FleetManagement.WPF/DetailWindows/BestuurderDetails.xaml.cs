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
    /// Interaction logic for BestuurderDetails.xaml
    /// </summary>
    public partial class BestuurderDetails : Window {

        private readonly Managers _managers;

        private Bestuurder _bestuurderDetail;

        public BestuurderDetails(Managers managers, Bestuurder bestuurder) {

            InitializeComponent();
            _managers = managers;
            _bestuurderDetail = bestuurder;

            //Controleer bestuurder om extra info op te vragen aan manager
            if (!bestuurder.HeeftBestuurderVoertuig || !bestuurder.HeeftBestuurderTankKaart)
            {
                //bestuurder = managers.BestuurderManager.BestuurderIncludes(bestuurder);  //interface 
            }

            if(bestuurder.HeeftBestuurderVoertuig)
            {
                StringBuilder stringBuilder = new(bestuurder.Voertuig.AutoModel.Merk + bestuurder.Voertuig.AutoModel.AutoModelNaam);
                stringBuilder.AppendLine("Chassisnr.: " + bestuurder.Voertuig.ChassisNummer);
                stringBuilder.AppendLine("Nummerplaat: " + bestuurder.Voertuig.NummerPlaat);

                HeeftVoertuig.Text = stringBuilder.ToString();
            }

            if(bestuurder.HeeftBestuurderTankKaart)
            {
                StringBuilder stringBuilder = new(" Kaartnr.: " + bestuurder.Tankkaart.TankKaartNummer);
                stringBuilder.AppendLine("Geldigheidsdatum: " + bestuurder.Tankkaart.GeldigheidsDatum.ToString("d/MM/yyyy"));

                if(bestuurder.Tankkaart.Actief) 
                { 
                    stringBuilder.AppendLine("Tankkaart is actief"); 
                }
                else
                {
                    if (bestuurder.Tankkaart.IsGeldigheidsDatumVervallen) { stringBuilder.AppendLine("Tankkaart is vervallen"); }
                    else { stringBuilder.AppendLine("Tankkaart is geblokkeerd"); } 
                }
   
                HeeftTankkaart.Text = stringBuilder.ToString();
            }

            //bind de bestuurder
            DataContext = bestuurder;
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void WijzigButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateBestuurder updateBestuurder = new(_managers, _bestuurderDetail)
            {
                Owner = Window.GetWindow(this),
                BestuurderDetail = _bestuurderDetail
            };

            bool? updatetet = updateBestuurder.ShowDialog();
            if (updatetet == true)
            {
                _bestuurderDetail = updateBestuurder.BestuurderDetail;
            }
        }
    }
}
