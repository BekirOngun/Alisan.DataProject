using System.Data.EntityClient;

namespace IKYS.Dal.Infrastructure
{
    public class Cryptographer
    {
        public static string GetConnectionString()
        {
            return "data source=AL-LOGODB;initial catalog=BI_REPORTS;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        }
        public static string GetConnectinStringMetadata()
        {
            return "res://*/LogoDataModel.csdl|res://*/LogoDataModel.ssdl|res://*/LogoDataModel.msl";
        }
        public static EntityConnection LogoConnection()
        {
            var decryptedConnectionString = GetConnectionString();
            var decryptedMetadata = GetConnectinStringMetadata();

            return new EntityConnection(new EntityConnectionStringBuilder()
            {
                Provider = "System.Data.SqlClient",
                Metadata = decryptedMetadata,
                ProviderConnectionString = decryptedConnectionString
            }.ConnectionString);
        }
    }
}
