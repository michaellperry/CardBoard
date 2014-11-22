using System.Linq;
using UpdateControls.Fields;
using UpdateControls.Correspondence.BinaryHTTPClient;
using CardBoard.Model;

namespace CardBoard
{
    public class HTTPConfigurationProvider : IHTTPConfigurationProvider
    {
        private Independent<Individual> _individual = new Independent<Individual>();

        public Individual Individual
        {
            get { return _individual; }
            set { _individual.Value = value; }
        }

        public HTTPConfiguration Configuration
        {
            get
            {
                string address = "http://correspondencedistributor.azurewebsites.net/";
                string apiKey = "Azure";
                int timeoutSeconds = 10;
                return new HTTPConfiguration(address, "CardBoard", apiKey, timeoutSeconds);
            }
        }

        public bool IsToastEnabled
        {
            get { return false; }
        }
    }
}
