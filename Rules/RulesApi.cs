using System;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace DataPrimer.Rules
{
    public class RulesApi
    {
        private readonly IRestClient _client;

        public RulesApi(IRestClient client, string url) {
            _client = client;
            _client.BaseUrl = new Uri(url);
            _client.UseNewtonsoftJson();
        }

        public W Execute<W>(string endpoint, object request, string propName) {
            var restRequest = new RestRequest(endpoint, DataFormat.Json);

            restRequest.AddJsonBody(request);
            var response = _client.Post(restRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) {
                throw new RulesApiException(response.ErrorMessage);
            }
 
            // Parsing JSON content into element-node JObject
            var jObject = JObject.Parse(response.Content);
            var result = jObject.Value<W>(propName);

            return result;
        }

    }
}