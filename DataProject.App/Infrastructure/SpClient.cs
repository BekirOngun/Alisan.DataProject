using DataProject.Domain.Constants;
using Microsoft.SharePoint.Client;
using System.Linq;
using System.Security;
using System.Web.Configuration;

namespace DataProject.App.Infrastructure
{
    public class SpClientContext : ClientContext
    {
        static SpClientContext spClient;

        SpClientContext(string url)
            : base(url)
        {

        }

        public static SpClientContext Client()
        {
            if (spClient == null)
                spClient = new SpClientContext(ProjectConstants.BASE_URL);

            var passwordList = WebConfigurationManager.AppSettings["Password"].ToList();
            var securePassword = new SecureString();

            passwordList.ForEach(t =>
            {
                securePassword.AppendChar(t);
            });

            spClient.Credentials = new SharePointOnlineCredentials(WebConfigurationManager.AppSettings["UserName"], securePassword);
            return spClient;
        }
    }
}
