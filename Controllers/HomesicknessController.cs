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
        public enum Interval {day, week, all};
        private static RecordService _recordService;

        public HomesicknessController(RecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet("/")]
        public IActionResult GetDefault()
        {
            return Redirect("/homesickness/week");
        }

        [HttpGet("homesickness/{timespan}")]
        public ViewResult Charts(Interval timespan)
        {
            ViewData.Add("timespan", timespan.ToString());

            List<Record> records;
            switch (timespan)
            { 
                case Interval.day:
                    var day = new TimeSpan(1, 0, 0, 0);
                    records = _recordService.GetFor(day);
                    break;
                case Interval.week:
                    var week = new TimeSpan(7, 0, 0, 0);
                    records = _recordService.GetFor(week);
                    break;
                default:
                    records = _recordService.GetAll();
                    break;
            }
            
            PrepareAreaChart(records, ViewData);
            PrepareBarChart(records, ViewData);

            return View("Views/Charts.cshtml");
        }

        private static void PrepareAreaChart(List<Record> records,
                                         Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary ViewData)
        {
            var bpTemps = records.AsQueryable().Select(r => r.BpTemperature).ToArray();
            var csTemps = records.AsQueryable().Select(r => r.CsTemperature).ToArray();
            var times = records.AsQueryable().Select(r => r.Time.ToJavascriptReadable()).ToArray();


            ViewData.Add("bpTemps", bpTemps);
            ViewData.Add("csTemps", csTemps);
            ViewData.Add("times", times);
            ViewData.Add("dataPointsNumber", times.Count());
        }

        private static void PrepareBarChart(List<Record> records,
                                        Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary ViewData)
        {
            Record latest = records.AsQueryable().OrderByDescending(r => r.Time).First();
            Record worst = _recordService.GetWorst();

            ViewData.Add("latestAnnotation", ("Bp " + latest.BpTemperature + "°C, " +
                                              "Cs " + latest.CsTemperature + "°C"));
            ViewData.Add("latestIndex", (latest.Index));

            ViewData.Add("worstAnnotation", (worst.Time.Date.ToShortDate() + ": " +
                                                "Bp " + worst.BpTemperature + "°C, " +
                                                "Cs " + worst.CsTemperature + "°C"));
            ViewData.Add("worstIndex", (worst.Index));
        }
    }
}