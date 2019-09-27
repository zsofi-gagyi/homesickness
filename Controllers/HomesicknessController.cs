using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HomesicknessVisualiser.Models;
using HomesicknessVisualiser.Services;
using Microsoft.Extensions.Logging;

namespace HomesicknessVisualiser.Controllers
{
    public class HomesicknessController : Controller
    {
        public enum Interval { day, week, all };
        private static RecordService _recordService;

        public HomesicknessController(RecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet("/")]
        public IActionResult Default()
        {
            return Redirect("/day");
        }

        [HttpGet("/{interval}")]
        public IActionResult Charts(Interval interval)
        {
            Tuple<List<Record>, bool> recordsOrRedirected = GetRecordsOrRedirected(interval);
            bool isRedirected = recordsOrRedirected.Item2;
            if (isRedirected)
            {
                TempData["redirected"] = true;
                interval = interval.Equals(Interval.day) ? Interval.week : Interval.all;
                return Redirect("/" + interval);
            }

            List<Record> records = recordsOrRedirected.Item1;
            ViewData.Add("interval", interval.ToString());

            if (records.Count < 2)
            {
                ViewData.Add("errorMessage", "There are not enough records to display. We keep collecting them, please " +
                    "come back soon!");
                return View("Views/Charts.cshtml");
            }

            if (TempData["redirected"] != null)
            {
                ViewData.Add("errorMessage", "Request redirected because there are not enough records to display for " +
                    "the specified period. We keep collecting them, please come back soon!");
            }

            PrepareAreaChart(records, ViewData);
            PrepareBarChart(records, ViewData);

            return View("Views/Charts.cshtml");
        }

        private Tuple<List<Record>, bool> GetRecordsOrRedirected(Interval interval){
            List<Record> records;
            bool isRedirected = false;

            if ((int)interval < 2)
            {
                bool recordsWereFoundForInterval = TryFindRecords(interval, out records);
                if (!recordsWereFoundForInterval)
                {
                    isRedirected = true;
                }
            }
            else
            {
                try
                {
                    records = _recordService.GetAll();
                }
                catch
                {
                    records = new List<Record>();
                }
            }

            return Tuple.Create(records, isRedirected);
        }

        private static bool TryFindRecords(Interval interval, out List<Record> records)
        {
            var timespan = interval.Equals(Interval.day) ? new TimeSpan(1, 0, 0, 0) : new TimeSpan(7, 0, 0, 0);
            try
            {
                records = _recordService.GetFor(timespan);
            }
            catch (Exception e)
            {
                records = new List<Record>();
                return false;
            }

            if (records.Count < 2)
            {
                return false;
            }

            return true;
        }

        private static void PrepareAreaChart(List<Record> records,
                                             Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary ViewData)
        {
            var bpTemps = records.Select(r => r.BpTemperature).ToArray();
            var csTemps = records.Select(r => r.CsTemperature).ToArray();
            var times = records.Select(r => r.Time.ToJavascriptReadable()).ToArray();

            ViewData.Add("bpTemps", bpTemps);
            ViewData.Add("csTemps", csTemps);
            ViewData.Add("times", times);
            ViewData.Add("dataPointsNumber", times.Count());
        }

        private static void PrepareBarChart(List<Record> records,
                                            Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary ViewData)
        {
            Record latest = records.OrderByDescending(r => r.Time).First();
            Record worst = _recordService.GetWorst();

            ViewData.Add("latestAnnotation", ("Bp " + latest.BpTemperature + "°C,\n" +
                                              "Cs " + latest.CsTemperature + "°C"));
            ViewData.Add("latestIndex", (latest.Index));

            ViewData.Add("worstAnnotation", (worst.Time.Date.ToShortDate() + ": \n" +
                                                "Bp " + worst.BpTemperature + "°C,\n" +
                                                "Cs " + worst.CsTemperature + "°C"));
            ViewData.Add("worstIndex", (worst.Index));
        }
    }
}