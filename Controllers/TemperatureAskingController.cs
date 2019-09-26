using HomesicknessVisualiser.Models;
using HomesicknessVisualiser.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomesicknessVisualiser.Controllers
{
    public class TemperatureAskingController : Controller
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly RecordService _recordService;

        public TemperatureAskingController(ILogger<TemperatureAskingController> logger, IHttpClientFactory httpClientFactory, RecordService recordService)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("temperatureGetter");
            _recordService = recordService;
        }

        [HttpGet("/ask")]
        public async Task Ask()
        {
            _logger.LogInformation("beginning to ask about temperatures");

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

            int index = IndexCalculator.CalculateIndex(bpTemp, csTemp);

            SaveRecord(bpTemp, csTemp, index);
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

        private void SaveRecord(float bpTemp, float csTemp, int index)
        {
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
    }
}