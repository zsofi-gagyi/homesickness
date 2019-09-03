using HomesicknessVisualiser.Models;
using HomesicknessVisualiser.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomesicknessVisualiser.Controllers
{
    public class TemperatureAsker 
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly RecordService _recordService;

        public TemperatureAsker(ILogger<TemperatureAsker> logger, IHttpClientFactory httpClientFactory, RecordService recordService)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("temperatureGetter");
            _recordService = recordService;
        }

        public async Task Ask()
        {
            string result = "";
            JObject objectResult = null;
            try
            { 
            result = await _client.GetStringAsync("");
            objectResult = JObject.Parse(result);

            _logger.LogInformation("response from the weather API was received and parsed");
            }
            catch (Exception e)
            {
                _logger.LogWarning("connection to the weather API or parsing the result was unsuccesful: " + e.Message);
                return;
            }

            float bpTemp, csTemp;
            try
            {
                bpTemp = GetTemperatureFor(objectResult, 0);
                csTemp = GetTemperatureFor(objectResult, 1);
                _logger.LogInformation("response processed into temperatures");
            }
            catch
            {
                _logger.LogWarning("the response has an unexpected format and could not be used: " + result);
                return;
            }

            int index = indexCalculator.getIndex(bpTemp, csTemp);
            try
            {
                _recordService.Save(
                    new Record
                    {
                        BpTemperature = bpTemp,
                        CsTemperature = csTemp,
                        Time = DateTime.Now,
                        Index = index
                    }
                );
                _logger.LogInformation("record saved");
            }
            catch
            {
                _logger.LogWarning("record could not be saved");
            }
        }

        private static float GetTemperatureFor(JObject objectResult, int i)
        {
            float tempInKelvin;
            try
            {
                tempInKelvin = objectResult["list"][i]["main"]["temp"].Value<float>();
            } 
            catch
            {
                throw;
            }

            float tempWithManyDecimals = tempInKelvin - 273.15f;
            return MathF.Truncate(tempWithManyDecimals * 10) / 10F;
        }
    }
}