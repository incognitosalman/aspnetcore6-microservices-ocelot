using Gateway.Identity.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Identity.Routing
{
    public class Router
    {
        public List<ModelRoute> Routes { get; set; }
        public Destination DestinationService { get; set; }

        // Read json file in constructor and extract configuration
        public Router(string routeConfigFilePath)
        {
            dynamic router = JsonReader.LoadFromFile<dynamic>(routeConfigFilePath);

            Routes = JsonReader.Deserialize<List<ModelRoute>>(Convert.ToString(router.routes));
            DestinationService = JsonReader.Deserialize<Destination>(Convert.ToString(router.authenticationService));
        }

        // Create Business Logic here to create destination URL/API after authentication
        public async Task<HttpResponseMessage> RouteRequest(HttpRequest request)
        {
            string path = request.Path.ToString();
            string basePath = '/' + path.Split('/')[1];

            // Redirect class to inner API/service
            Destination destination;
            try
            {
                destination = Routes.First(r => r.Endpoint.Equals(basePath)).Destination;
            }
            catch(Exception ex)
            {
                return ConstructErrorMessage("The path could not be found.");
            }

            if (destination.RequiresAuthentication) // Required authentication
            {
                string token = request.Headers["Authorization"]; // Fetch Token

                if (string.IsNullOrWhiteSpace(token))
                {
                    return ConstructErrorMessage("This API URL need Token Authentication.");
                }
                //Redirect to URL
                HttpResponseMessage authResponse = await destination.SendRequest(request, destination.RequiresAuthentication);
                if (!authResponse.IsSuccessStatusCode)
                {
                    return ConstructErrorMessage("Authentication failed OR Token Expired - " +
                    authResponse.RequestMessage.ToString());
                }
            }
            return await destination.SendRequest(request, destination.RequiresAuthentication); // By pass
        }
        private HttpResponseMessage ConstructErrorMessage(string error)
        {
            HttpResponseMessage errorMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(error)
            };
            return errorMessage;
        }
    }
}
