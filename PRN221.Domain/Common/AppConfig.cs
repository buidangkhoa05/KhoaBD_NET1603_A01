using PRN221.Domain.Models;

namespace PRN221.Domain.Common
{
    public class AppConfig
    {
        public static ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
        public static Admin Admin { get; set; } = new Admin();
        public static Customer Customer { get; set; }
    }

    public class  ConnectionStrings
    {
        public string DefaultConnection { get; set; } = "Server=localhost,1433;Database=MyStore;User Id=sa;Password=12345;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
