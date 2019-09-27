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
        private readonly HttpClient _client;
        private readonly RecordService _recordService;

        public TemperatureAskingController(IHttpClientFactory httpClientFactory, RecordService recordService)
        {
            _client = httpClientFactory.CreateClient("temperatureGetter");
            _recordService = recordService;
        }

        [HttpGet("/ask")]
        public async Task Ask()
        {
            JObject objectResult = await GetObjectResult();
            if (objectResult == null)
            {
                return;
            }

            float bpTemp, csTemp;
            try
            {
                bpTemp = GetTemperatureFor(objectResult, 0);
                csTemp = GetTemperatureFor(objectResult, 1);
            }
            catch
            {
                return;
            }

            int index = IndexCalculator.CalculateIndex(bpTemp, csTemp);
            SaveRecord(bpTemp, csTemp, index);
        }

        private async Task<JObject> GetObjectResult()
        {
            try
            {
                string result = await _client.GetStringAsync("");
                JObject objectResult = JObject.Parse(result);
                return objectResult;
            }
            catch (Exception e)
            {
                return null;
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

        private void SaveRecord(float bpTemp, float csTemp, int index)
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
        }
    }
}