//using FleetManagement.Interfaces;
using FleetManagement.Manager;
using FleetManagement.Manager.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FleetManagement.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UnitOfManager Manager { get; set; }

        public App()
        {
            //ConnectionString inladen
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("app.json").Build();
            string connectionString = config.GetConnectionString("FleetManagementConnectionString");

            //ADO instantie aanmaken
            Interfaces.IUnitOfRepository repository = new ADO.Repositories.UnitOfRepository(connectionString);

            //Manager instantie aamaken & Autotypes inladen
            Manager = new UnitOfManager(repository)
            {
                AutoTypes = config.GetSection("autotypes").AsEnumerable()
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow(Manager) { Title = "FleetManagement App" };
            MainWindow.Show();
        }
    }
}
