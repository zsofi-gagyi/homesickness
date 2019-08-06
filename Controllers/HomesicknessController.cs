using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HomesicknessVisualiser.Models;
using HomesicknessVisualiser.Services;

namespace HomesicknessVisualiser.Controllers
{
    public class HomesicknessController : Controller
    {
        private readonly RecordService _recordService;

        public HomesicknessController(RecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet("homesickness/{timespan}")]
        public ViewResult GetFor(string timespan)
        {
            List<Record> records;

            if (timespan == "all")
            {
                records = _recordService.GetAll();
            }
            else
            {
                TimeSpan time;
                if (timespan == "week")
                {
                    time = new TimeSpan(7, 0, 0, 0);
                }
                else
                {
                    time = new TimeSpan(1, 0, 0, 0);
                }
                records = _recordService.GetFor(time); 
            }

            Record latest = records.AsQueryable().OrderByDescending(r => r.Time).First();
            Record worst = _recordService.GetWorst();

            var bpTemps = records.AsQueryable().Select(r => r.BpTemperature).ToArray();
            var csTemps = records.AsQueryable().Select(r => r.CsTemperature).ToArray();
            var times = records.AsQueryable().Select(r => r.Time.ToJavascriptReadable()).ToArray();

            ViewData.Add("timespan", timespan);
            ViewData.Add("bpTemps", bpTemps);
            ViewData.Add("csTemps", csTemps);
            ViewData.Add("times", times);
            ViewData.Add("dataPointsNumber", times.Count());

            ViewData.Add("latestAnnotation", (  "Bp " + latest.BpTemperature + "°C, " +
                                                "Cs " + latest.CsTemperature + "°C"));
            ViewData.Add("latestIndex", (latest.Index));

            ViewData.Add("worstAnnotation", (worst.Time.Date.ToShortDate() + ": " +
                                                "Bp " + worst.BpTemperature + "°C, " +
                                                "Cs " + worst.CsTemperature + "°C"));
            ViewData.Add("worstIndex", (worst.Index));

            return View("Views/charts.cshtml");
        }
    }
}