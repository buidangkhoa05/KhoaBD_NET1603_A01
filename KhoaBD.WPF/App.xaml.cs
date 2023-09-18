
using KhoaBD.WPF.Windows;
using Microsoft.EntityFrameworkCore;
using PRN221.Application.Service.Implement;
using PRN221.Application.Service.Interface;
using PRN221.Domain.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace KhoaBD.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public IConfiguration _configuration;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {

            services.AddDbContext<FucarRentingManagementContext>(options => options.UseSqlServer(AppConfig.ConnectionStrings.DefaultConnection));

            services.AddTransient(typeof(LoginWindow));
            services.AddTransient(typeof(AdminWindow));
            services.AddTransient(typeof(CustomerWindow));
            
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddTransient(typeof(IAuthenService), typeof(AuthenServivce));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            AppConfig.ConnectionStrings.DefaultConnection = _configuration.GetConnectionString("DefaultConnection");

            if (AppConfig.Admin == null)
            {
                AppConfig.Admin = new Admin();
            }

            _configuration.Bind("AdminConfig", AppConfig.Admin);

            if (AppConfig.ConnectionStrings.DefaultConnection == null)
            {
                throw new Exception("Khong doc duoc config");

            }

            var windowCarManagement = _serviceProvider.GetService<LoginWindow>();
            windowCarManagement?.Show();
        }
    }
}
