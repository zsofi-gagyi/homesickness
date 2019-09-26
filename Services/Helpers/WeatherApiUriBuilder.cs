using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace HomesicknessVisualiser.Services
{
    public static class WeatherApiUriBuilder
    {
        public static string Build()
        {
            var weatherProvider = "http://api.openweathermap.org/data/2.5";
            var requestType = "/group";
            var csikszeredaId = "3054643";
            var budapestId = "673441";
            var appId = "47faf317899505d554ab7e24fec7b703";

            var requestParams = new Dictionary<string, string>();
            requestParams.Add("id", csikszeredaId + "," + budapestId);
            requestParams.Add("APPID", appId);

            var baseUri = weatherProvider + requestType;
            return QueryHelpers.AddQueryString(baseUri, requestParams);
        }
    }
}
