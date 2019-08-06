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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RecordService _recordService;

        public TemperatureAsker(IHttpClientFactory httpClientFactory, RecordService recordService)
        {
            
            _httpClientFactory = httpClientFactory;
            _recordService = recordService;
        }

        public async Task Ask()
        {
            var client = _httpClientFactory.CreateClient("temperatureGetter");
            string result = await client.GetStringAsync("");
            var objectResult = JObject.Parse(result);

            float bpTempInKelvin = objectResult["list"][0]["main"]["temp"].Value<float>();
            float bpTempWithManyDecimals = bpTempInKelvin - 273.15f;
            float bpTemp = MathF.Truncate(bpTempWithManyDecimals * 10) / 10F;

            float csTempInKelvin = objectResult["list"][1]["main"]["temp"].Value<float>();
            float csTempWithManyDecimals = csTempInKelvin - 273.15f;
            float csTemp = MathF.Truncate(csTempWithManyDecimals * 10) / 10F;

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
    }
}