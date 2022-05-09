using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Identity.Routing
{
    public class Destination
    {
        public string Uri { get; set; }
        public bool RequiresAuthentication { get; set; }

        public Destination(string path, bool requiresAuthentication)
        {
            Uri = path;
            RequiresAuthentication = requiresAuthentication;
        }

        public Destination(string uri)
            : this(uri, false)
        {
        }

        private Destination()
        {
            Uri = "/";
            RequiresAuthentication = false;
        }

        // READ / WRITE to destination URL/API/ inner services
        public async Task<HttpResponseMessage> SendRequest(HttpRequest request, bool isTokenBased)
        {
            // REQUEST
            string requestContent;
            using (Stream receiveStream = request.Body)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    requestContent = readStream.ReadToEnd();
                }
            }

            HttpClientHandler handler = new HttpClientHandler();

            // RESPONSE
            using (var newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request)))
            {
                newRequest.Content = new StringContent(requestContent, Encoding.UTF8, request.ContentType);
                using (var client = new HttpClient(handler))
                {
                    if (isTokenBased)
                    {
                        string token = request.Headers["Authorization"]; // Fetch Token
                        client.DefaultRequestHeaders.Add("Authorization", token);
                    }
                    return await client.SendAsync(newRequest);
                }
            }
        }

        // Create Final URL here to redirect to other Microservices
        private string CreateDestinationUri(HttpRequest request)
        {
            string requestPath = request.Path.ToString();
            string queryString = request.QueryString.ToString();

            string endpoint = "";
            string[] endpointSplit = requestPath.Substring(1).Split('/');

            if (endpointSplit.Length > 1)
                endpoint = endpointSplit[1];

            var finalURL =  Uri + endpoint + queryString;
            return finalURL;
        }
    }
}