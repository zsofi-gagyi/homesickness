using HomesicknessVisualiser.Models;
using HomesicknessVisualiser.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomesicknessVisualiser.Controllers
{
    public class TemperatureAsker 
    {
        private readonly HttpClient _client;
        private readonly RecordService _recordService;

        public TemperatureAsker(IHttpClientFactory httpClientFactory, RecordService recordService)
        {
            
            _client = httpClientFactory.CreateClient("temperatureGetter");
            _recordService = recordService;
        }

        public async Task Ask()
        {
            string result = await _client.GetStringAsync("");
            var objectResult = JObject.Parse(result);

            float bpTemp = GetTemperatureFor(objectResult, 0);
            float csTemp = GetTemperatureFor(objectResult, 1);

            int index = indexCalculator.getIndex(bpTemp, csTemp);

            _recordService.Save(
                new Record
                {
                    BpTemperature = bpTemp,
                    CsTemperature = csTemp,
                    Time = DateTime.Now,
                    Index = index
                }
            );
        }

        private static float GetTemperatureFor(JObject objectResult, int i)
        {
            float tempInKelvin = objectResult["list"][i]["main"]["temp"].Value<float>();
            float tempWithManyDecimals = tempInKelvin - 273.15f;
            return MathF.Truncate(tempWithManyDecimals * 10) / 10F;
        }
    }
}