using System;
using Newtonsoft.Json.Linq;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

using DataPrimer.Api;

namespace DataPrimer.Simulation
{
    public class SimulationApi
    {
        private readonly IRestClient _client;
        private readonly string _password;

        public SimulationApi(IRestClient client, string url, string password) {
            _client = client;
            _client.BaseUrl = new Uri(url);
            _client.UseNewtonsoftJson();
            _password = password;
        }

        public string Execute(string endpoint, object request) {
            var restRequest = new RestRequest(endpoint, DataFormat.Json);

            restRequest.AddJsonBody(request);
            restRequest.AddHeader("password", _password);
            var response = _client.Post(restRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) {
                throw new ApiException(response.Content);
            }

            return "Ok";
        }

    }
}