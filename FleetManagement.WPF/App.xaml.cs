//using FleetManagement.Interfaces;
using FleetManagement.Manager;
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
        public Managers Managers { get; private set; }

        public App()
        {
            //ConnectionString inladen
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("app.json").Build();
            string connectionString = config.GetConnectionString("FleetManagementConnectionString");

            //ADO instantie aanmaken
            Interfaces.IRepositories repositories = new ADO.Repositories.Repositories(connectionString);

            //Manager instantie aamaken & Autotypes inladen
            Managers = new Managers(repositories)
            {
                AutoTypes = config.GetSection("autotypes").AsEnumerable().Where(a => !string.IsNullOrEmpty(a.Value))
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow(Managers) { Title = "FleetManagement App" };
            MainWindow.Show();
        }
    }
}
